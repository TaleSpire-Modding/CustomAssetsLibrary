using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace CustomAssetsKind
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

        internal static ConfigEntry<LogLevel> LogLevel { get; set; }
        internal static Harmony harmony;

        public static void DoPatching()
        {
            harmony = new Harmony(Guid);
            harmony.PatchAll();
            if (LogLevel.Value > CustomAssetsKind.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Patched.");
        }

        public static void UnPatch()
        {
            harmony.UnpatchSelf();
            if (LogLevel.Value > CustomAssetsKind.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Unpatched.");
        }

        public static void DoConfig(ConfigFile Config)
        {
            LogLevel = Config.Bind("Logging", "Level", CustomAssetsKind.LogLevel.Low);
            if (LogLevel.Value > CustomAssetsKind.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Config Bound.");
        }

        private void Awake()
        {
            DoConfig(Config);
            DoPatching();
            if (LogLevel.Value > CustomAssetsKind.LogLevel.None) Debug.Log($"Custom Asset Library Plugin:{Name} is Active.");
        }
    }
}