using System.IO;
using System;
using System.Text;
using Bounce.Singletons;
using Bounce.Unmanaged;
using CustomAssetsLoader;
using CustomAssetsLoader.ReflecExt;
using HarmonyLib;
using MD5 = System.Security.Cryptography.MD5;
using UnityEngine;
using Newtonsoft.Json;
using CustomAssetsCompiler.CoreDTO;

// ReSharper disable once CheckNamespace
namespace CustomAssetsLibrary.Patches
{
    
    [HarmonyPatch(typeof(AssetLoadManager), "OnInstanceSetup")]
    public class AssetDbOnSetupInternalsPatch
    {
        private static string dirPlugin = BepInEx.Paths.PluginPath;

        /// <summary>
        /// Public for Testing only
        /// </summary>
        public static void Postfix()
        {
           foreach (string directory in Directory.GetDirectories(dirPlugin))
                    LoadDirectory(directory);
        }

        /// <summary>
        /// Loads extra directory into Asset DB.
        /// This is public to allow other plugins to refresh post setup
        /// </summary>
        public static void LoadDirectory(string directory)
        {
            if (!File.Exists(Path.Combine(directory, "index.json"))) return; // Needs an index
            if (CustomAssetLoader.LogLevel.Value >= LogLevel.Low)
                Debug.Log($"Index found in: {directory}");

            string text = File.ReadAllText(Path.Combine(directory, "index.json"));
            var index = JsonConvert.DeserializeObject<CustomAssetsPlugin.Data.Index>(text);
            var guid = new NGuid(index.assetPackId);
            
            var instance = SimpleSingletonBehaviour<AssetLoadManager>.Instance;
            
            instance.call("LoadInternalAssetPack", new object[] { guid, directory });

            if (CustomAssetLoader.LogLevel.Value >= LogLevel.Low)
                Debug.Log($"Pack {guid} Loaded");

        }

        /// <summary>
        /// Create an NGuid from a string via MD5 Hash
        /// </summary>
        /// <param name="id">text to get hashed</param>
        /// <returns>NGuid of the string</returns>
        internal static NGuid GenerateID(string id)
            => new NGuid(Guid.Parse(CreateMD5(id)));

        private static string CreateMD5(string input)
        {
            // Don't hash if can parse
            if (Guid.TryParse(input, out var result)) return input;
            
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