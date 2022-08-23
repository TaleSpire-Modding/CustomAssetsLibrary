using System.Collections.Generic;
using System.IO;
using Bounce.Unmanaged;
using CustomAssetsCompiler.CoreDTO;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;

namespace CustomAssetsKind.Patches
{
    [HarmonyPatch(typeof(AssetLoadManager), "OnInstanceSetup")]
    public class AssetDbOnSetupInternalsPatch
    {
        private static string dirPlugin = BepInEx.Paths.PluginPath;
        private static readonly Dictionary<NGuid, string> _registeredInternalAssetPacksInfo = new Dictionary<NGuid, string>();

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
            if (!File.Exists(Path.Combine(directory, "index.json")) || !File.Exists(Path.Combine(directory, "customIndex"))) return; // Needs a custom index
            if (CustomAssetKindPlugin.LogLevel.Value >= LogLevel.Low)
                Debug.Log($"Index found in: {directory}");

            string text = File.ReadAllText(Path.Combine(directory, "index.json"));
            var index = JsonConvert.DeserializeObject<CustomAssetsPlugin.Data.Index>(text);
            var guid = new NGuid(index.assetPackId);

            // TODO
            // Handle loading of customPackIndex
            
            if (CustomAssetKindPlugin.LogLevel.Value >= LogLevel.Low)
                Debug.Log($"Pack {guid} Loaded");
        }
    }
}
