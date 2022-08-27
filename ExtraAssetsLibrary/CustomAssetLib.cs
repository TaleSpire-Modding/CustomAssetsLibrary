using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Bounce.Unmanaged;
using CustomAssetsLibrary.DTO;
using CustomAssetsLibrary.Patches;
using HarmonyLib;
using LordAshes;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

namespace CustomAssetsLibrary
{
    public enum LogLevel
    {
        None,
        Low,
        Medium,
        High,
        All,
    }

    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(SmartConvert.Guid)]
    public class CustomAssetLib : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.PM.plugins.CustomAssetLib";
        public const string Version = "1.1.1.0";
        private const string Name = "Plugin Masters' Custom Asset Library";

        internal static ConfigEntry<bool> AutoClear { get; set; }
        internal static ConfigEntry<bool> RunTestsConfig { get; set; }
        internal static ConfigEntry<LogLevel> LogLevel { get; set; }

        internal static Harmony harmony;

        public static void DoPatching()
        {
            harmony = new Harmony(Guid);
            harmony.PatchAll();
            if (LogLevel.Value > CustomAssetsLibrary.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Patched.");
        }

        public static void UnPatch()
        {
            harmony.UnpatchSelf();
            if (LogLevel.Value > CustomAssetsLibrary.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Unpatched.");
        }

        public static void DoConfig(ConfigFile Config)
        {
            AutoClear = Config.Bind("Mini Loading", "Auto Clear Failed Minis", false);
            LogLevel = Config.Bind("Logging", "Level", CustomAssetsLibrary.LogLevel.Low);
            RunTestsConfig = Config.Bind("Tests", "Execute", false);
            if (LogLevel.Value > CustomAssetsLibrary.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Config Bound.");
        }

        private void Awake()
        {
            DoConfig(Config);
            DoPatching();
            if (LogLevel.Value > CustomAssetsLibrary.LogLevel.None) Debug.Log($"Custom Asset Library Plugin:{Name} is Active."); 
            // if (RunTestsConfig.Value) RunTests();
        }

        // Public interface method to generate a binary INDEX for CAP
        public static void Generate(string directory)
        {
            var pack = new AssetPackContent();
            pack.FromJson(directory);
            if (LogLevel.Value > CustomAssetsLibrary.LogLevel.None) Debug.Log($"Added {Path.Combine(directory, "index")}");
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
            => AssetDbOnSetupInternalsPatch.GenerateID(id);
    }
}