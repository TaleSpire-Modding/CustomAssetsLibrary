using System.IO;
using System.Text;
using System.Threading;
using Bounce.Unmanaged;
using CustomAssetsCompiler.CoreDTO;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Jobs;
using UnityEngine;
using MD5 = System.Security.Cryptography.MD5;

namespace CustomAssetsCompiler
{
    public sealed class TaleWeaverCompiler
    {
        public const string Version = "1.0.0.0";

        public enum impl
        {
            Sync,
            Thread,
            ThreadComplete,
            Burst,
            BurstComplete
        }

        // Public interface method to generate a binary INDEX for CAP
        public static void Generate(string directory, impl burst = impl.Sync, LogLevel logLevel = LogLevel.None)
        {
            switch (burst)
            {
                case impl.Sync:
                    WritePack(directory, logLevel);
                    break;
                case impl.Burst:
                    new Packer
                    {
                        directory = directory,
                        logLevel = logLevel
                    }.Schedule();
                    break;
                case impl.BurstComplete:
                    new Packer
                    {
                        directory = directory,
                        logLevel = logLevel
                    }.Schedule().Complete();
                    break;
                case impl.Thread:
                    new Thread(() =>
                    {
                        WritePack(directory,logLevel);
                    }).Start();
                    break;
                case impl.ThreadComplete:
                    var t = new Thread(() =>
                    {
                        WritePack(directory, logLevel);
                    });
                    t.Start();
                    t.Join();
                    break;
            }
        }

        public static void WritePack(string directory, LogLevel logLevel = LogLevel.None)
        {
            var content = new AssetPackContent();
            content.FromJson(directory);
            if (logLevel > LogLevel.None) Debug.Log($"Added {Path.Combine(directory, "index")}");
            
            var blobref = content.GenerateBlobAssetReference();
            var indexDestinationLocation = Path.Combine(directory, "index");
            var writer = new StreamBinaryWriter(indexDestinationLocation);
            writer.Write(blobref);
            File.WriteAllText(Path.Combine(directory, "assetpack.id"), $"{content.assetPackString},{content.Name}");
        }

        [BurstCompile]
        internal struct Packer : IJob
        {
            [ReadOnly]
            internal string directory;

            [ReadOnly]
            internal LogLevel logLevel;
            public void Execute()
            {
                var content = new AssetPackContent();
                content.FromJson(directory);
                if (logLevel > LogLevel.None) Debug.Log($"Added {Path.Combine(directory, "index")}");

                var blobref = content.GenerateBlobAssetReference();
                var indexDestinationLocation = Path.Combine(directory, "index");
                var writer = new StreamBinaryWriter(indexDestinationLocation);
                writer.Write(blobref);
                File.WriteAllText(Path.Combine(directory, "assetpack.id"), $"{content.assetPackString},{content.Name}");
            }
        }

        /// <summary>
            /// Create an NGuid from a string via MD5 Hash
            /// </summary>
            /// <param name="id">text to get hashed</param>
            /// <returns>NGuid of the string</returns>
            public static NGuid GenerateID(string id)
            => new NGuid(System.Guid.Parse(CreateMD5(id)));

        private static string CreateMD5(string input)
        {
            // Don't hash if can parse
            if (System.Guid.TryParse(input, out _)) return input;

            // Use input string to calculate MD5 hash
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }
    }
}