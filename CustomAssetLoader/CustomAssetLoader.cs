using BepInEx;
using BepInEx.Configuration;
using Bounce.Unmanaged;
using CustomAssetsLibrary.Patches;
using HarmonyLib;
using LordAshes;
using UnityEngine;

namespace CustomAssetsLoader
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
    public class CustomAssetLoader : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.PM.plugins.CustomAssetLoader";
        public const string Version = "1.0.0.0";
        private const string Name = "Plugin Masters' Custom Asset Loader";

        internal static ConfigEntry<bool> AutoClear { get; set; }
        internal static ConfigEntry<bool> RunTestsConfig { get; set; }
        internal static ConfigEntry<LogLevel> LogLevel { get; set; }
        internal static Harmony harmony;

        public static void DoPatching()
        {
            harmony = new Harmony(Guid);
            harmony.PatchAll();
            if (LogLevel.Value > CustomAssetsLoader.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Patched.");
        }

        public static void UnPatch()
        {
            harmony.UnpatchSelf();
            if (LogLevel.Value > CustomAssetsLoader.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Unpatched.");
        }

        public static void DoConfig(ConfigFile Config)
        {
            AutoClear = Config.Bind("Mini Loading", "Auto Clear Failed Minis", false);
            LogLevel = Config.Bind("Logging", "Level", CustomAssetsLoader.LogLevel.Low);
            RunTestsConfig = Config.Bind("Tests", "Execute", false);
            if (LogLevel.Value > CustomAssetsLoader.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Config Bound.");
        }

        private void Awake()
        {
            DoConfig(Config);
            DoPatching();
            if (LogLevel.Value > CustomAssetsLoader.LogLevel.None) Debug.Log($"Custom Asset Library Plugin:{Name} is Active.");
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