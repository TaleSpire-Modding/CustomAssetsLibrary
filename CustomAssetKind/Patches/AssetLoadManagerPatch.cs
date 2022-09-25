using System.Collections.Generic;
using System.IO;
using Bounce.Animation;
using Bounce.Unmanaged;
using CustomAssetsCompiler.CoreDTO;
using HarmonyLib;
using LordAshes;
using ModdingTales;
using UnityEngine;

namespace CustomAssetsKind.Patches
{
    [HarmonyPatch(typeof(AssetLoadManager), "OnInstanceSetup")]
    public sealed class AssetDbOnSetupInternalsPatch
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
            if (CustomAssetKindPlugin.LogLevelConfig.Value != ModdingUtils.LogLevel.None)
                Debug.Log($"Index found in: {directory}");

            var guid = GetGuidFromDirectory(directory);

            // TODO
            // Handle loading of customPackIndex
            
            if (CustomAssetKindPlugin.LogLevelConfig.Value != ModdingUtils.LogLevel.None)
                Debug.Log($"Pack {guid} Loaded");
        }

        private static NGuid GetGuidFromDirectory(string directory)
        {
            if (File.Exists(Path.Combine(directory, "assetpack.id")))
                return new NGuid(File.ReadAllText(Path.Combine(directory, "assetpack.id")));

            var text = File.ReadAllText(Path.Combine(directory, "index.json"));
            var index = SmartConvert.Json.DeserializeObject<CustomAssetsPlugin.Data.Index>(text);
            File.WriteAllText(Path.Combine(directory, "assetpack.id"), index.assetPackId);

            return new NGuid(index.assetPackId);
        }
    }
}
