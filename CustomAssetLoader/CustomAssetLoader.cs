using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Bounce.Unmanaged;
using CustomAssetsLibrary.Patches;
using HarmonyLib;
using ModdingTales;
using PluginUtilities;

namespace CustomAssetsLoader
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(SetInjectionFlag.Guid)]
    public sealed class CustomAssetLoader : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.PM.plugins.CustomAssetLoader";
        public const string Version = "1.1.1.0";
        private const string Name = "Custom Asset Loader";

        // internal static ConfigEntry<bool> AutoClear { get; set; }
        // internal static ConfigEntry<bool> RunTestsConfig { get; set; }
        internal static ConfigEntry<ModdingUtils.LogLevel> _logLevel { get; set; }
        internal static ConfigEntry<bool> abExperiment { get; set; }
        internal static Harmony harmony;
        internal static ConfigFile ConfigWriter;
        internal static ManualLogSource _logger;

        internal static ModdingUtils.LogLevel LogLevel => _logLevel.Value == ModdingUtils.LogLevel.Inherited ? ModdingUtils.LogLevelConfig.Value : _logLevel.Value;

        public static void DoPatching()
        {
            harmony = new Harmony(Guid);
            harmony.PatchAll();
            if (LogLevel > ModdingUtils.LogLevel.None) _logger.LogInfo($"Patched.");
        }

        public static void UnPatch()
        {
            harmony.UnpatchSelf();
            if (LogLevel > ModdingUtils.LogLevel.None) _logger.LogInfo($"Unpatched.");
        }

        public static void DoConfig(ConfigFile Config)
        {
            ConfigWriter = Config;
            // AutoClear = Config.Bind("Mini Loading", "Auto Clear Failed Minis", false);
            _logLevel = Config.Bind("Logging", "Log Level", ModdingUtils.LogLevel.Inherited);
            abExperiment = Config.Bind("UI", "Experimental Asset Browser", false);
            // RunTestsConfig = Config.Bind("Tests", "Execute", false);
            if (LogLevel > ModdingUtils.LogLevel.None) _logger.LogInfo($"Config Bound.");
        }

        private void Awake()
        {
            _logger = Logger;
            DoConfig(Config);
            DoPatching();
            if (LogLevel > ModdingUtils.LogLevel.None) _logger.LogInfo($"{Name} is Active.");
        }

        /// <summary>
        /// Create an NGuid from a string via MD5 Hash
        /// </summary>
        /// <param name="id">text to get hashed</param>
        /// <returns>NGuid of the string</returns>
        public static NGuid GenerateID(string id)
            => AssetLoadManagerOnInstanceSetupPatch.GenerateID(id);
    }
}