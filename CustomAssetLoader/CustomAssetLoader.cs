using BepInEx;
using BepInEx.Configuration;
using Bounce.Unmanaged;
using CustomAssetsLibrary.Patches;
using HarmonyLib;
using LordAshes;
using ModdingTales;
using PluginUtilities;
using UnityEngine;

namespace CustomAssetsLoader
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(SmartConvert.Guid)]
    [BepInDependency(SetInjectionFlag.Guid)]
    public sealed class CustomAssetLoader : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.PM.plugins.CustomAssetLoader";
        public const string Version = "1.0.0.0";
        private const string Name = "Custom Asset Loader";

        // internal static ConfigEntry<bool> AutoClear { get; set; }
        // internal static ConfigEntry<bool> RunTestsConfig { get; set; }
        internal static ConfigEntry<ModdingUtils.LogLevel> _logLevel { get; set; }
        internal static Harmony harmony;
        internal static ConfigFile ConfigWriter;

        internal static ModdingUtils.LogLevel LogLevel => _logLevel.Value == ModdingUtils.LogLevel.Inherited ? ModdingUtils.LogLevelConfig.Value : _logLevel.Value;

        public static void DoPatching()
        {
            harmony = new Harmony(Guid);
            harmony.PatchAll();
            if (LogLevel > ModdingUtils.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Patched.");
        }

        public static void UnPatch()
        {
            harmony.UnpatchSelf();
            if (LogLevel > ModdingUtils.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Unpatched.");
        }

        public static void DoConfig(ConfigFile Config)
        {
            ConfigWriter = Config;
            // AutoClear = Config.Bind("Mini Loading", "Auto Clear Failed Minis", false);
            _logLevel = Config.Bind("Logging", "Log Level", ModdingUtils.LogLevel.Inherited);
            // RunTestsConfig = Config.Bind("Tests", "Execute", false);
            if (LogLevel > ModdingUtils.LogLevel.None) Debug.Log($"Custom Asset Library Plugin: Config Bound.");
        }

        private void Awake()
        {
            DoConfig(Config);
            DoPatching();
            if (LogLevel > ModdingUtils.LogLevel.None) Debug.Log($"Custom Asset Library Plugin:{Name} is Active.");
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