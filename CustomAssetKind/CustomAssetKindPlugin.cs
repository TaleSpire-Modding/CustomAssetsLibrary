using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
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
    public class CustomAssetKindPlugin : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.PM.plugins.CustomAssetKindPlugin";
        public const string Version = "1.1.0.0";
        private const string Name = "Plugin Masters' Custom Asset Kind";

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
    }
}