using System.IO;
using System;
using System.Linq;
using System.Text;
using Bounce.Singletons;
using Bounce.Unmanaged;
using CustomAssetsLoader;
using CustomAssetsLoader.ReflecExt;
using HarmonyLib;
using MD5 = System.Security.Cryptography.MD5;
using UnityEngine;
using CustomAssetsCompiler.CoreDTO;
using LordAshes;
using ModdingTales;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace CustomAssetsLibrary.Patches
{
    
    [HarmonyPatch(typeof(AssetLoadManager), "OnInstanceSetup")]
    public sealed class AssetDbOnSetupInternalsPatch
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
            if (CustomAssetLoader.LogLevel != ModdingUtils.LogLevel.None)
                Debug.Log($"Index found in: {directory}");

            var guid = GetGuidFromDirectory(directory, out var assetPackName);

            // var packConfig = CustomAssetLoader.ConfigWriter.Bind("Load Asset Pack", assetPackName, true);

            // if (!packConfig.Value) return;
            var instance = SimpleSingletonBehaviour<AssetLoadManager>.Instance;
            instance.call("LoadInternalAssetPack", new object[] { guid, directory });
            if (CustomAssetLoader.LogLevel != ModdingUtils.LogLevel.None)
                Debug.Log($"Pack {guid} Loaded");
        }

        private static NGuid GetGuidFromDirectory(string directory, out string assetPackName)
        {
            string text;
            CustomAssetsPlugin.Data.Index index;

            if (File.Exists(Path.Combine(directory, "assetpack.id")))
            {
                var csv = File.ReadAllText(Path.Combine(directory, "assetpack.id")).Split(',').Select(s => s.Replace(",","")).ToArray();
                if (csv.Length < 2)
                {
                    text = File.ReadAllText(Path.Combine(directory, "index.json"));
                    index = JsonConvert.DeserializeObject<CustomAssetsPlugin.Data.Index>(text, CustomAssetDTO.Sentry.Utilities.options);
                    File.WriteAllText(Path.Combine(directory, "assetpack.id"), $"{index.assetPackId},{index.Name}");
                    assetPackName = index.Name;
                }
                else
                {
                    assetPackName = csv[1];
                }
                return new NGuid(csv[0]);
            }
            
            text = File.ReadAllText(Path.Combine(directory, "index.json"));
            index = JsonConvert.DeserializeObject<CustomAssetsPlugin.Data.Index>(text, CustomAssetDTO.Sentry.Utilities.options);
            File.WriteAllText(Path.Combine(directory, "assetpack.id"), $"{index.assetPackId},{index.Name}");
            assetPackName = index.Name;
            return new NGuid(index.assetPackId);
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