﻿using System.IO;
using System.Text;
using Bounce.Unmanaged;
using CustomAssetsCompiler.CoreDTO;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;
using MD5 = System.Security.Cryptography.MD5;

namespace CustomAssetsCompiler
{
    public class TaleWeaverCompiler
    {
        public const string Version = "1.0.0.0";
        
        // Public interface method to generate a binary INDEX for CAP
        public static void Generate(string directory, LogLevel logLevel = LogLevel.None)
        {
            var pack = new AssetPackContent();
            pack.FromJson(directory);
            if (logLevel > LogLevel.None) Debug.Log($"Added {Path.Combine(directory, "index")}");
            WritePack(directory, pack);
        }

        public static void WritePack(string directory, AssetPackContent content)
        { 
            var blobref = content.GenerateBlobAssetReference();
            var indexDestinationLocation = Path.Combine(directory, "index");
            var writer = new StreamBinaryWriter(indexDestinationLocation);
            writer.Write(blobref);
            File.WriteAllText(Path.Combine(directory, "assetpack.id"), content.assetPackString);
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