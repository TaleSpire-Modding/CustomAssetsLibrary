﻿using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ModdingTales;
using PluginUtilities;

namespace CustomAssetsKind
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(SetInjectionFlag.Guid)]
    public sealed class CustomAssetKindPlugin : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.PM.plugins.CustomAssetKindPlugin";
        public const string Version = "1.1.0.0";
        private const string Name = "Custom Asset Kind";

        internal static ConfigEntry<ModdingUtils.LogLevel> LogLevelConfig { get; set; }
        internal static Harmony harmony;
        internal static ManualLogSource PluginLogger;

        internal static ModdingUtils.LogLevel LogLevel => LogLevelConfig.Value == ModdingUtils.LogLevel.Inherited ? ModdingUtils.LogLevelConfig.Value : LogLevelConfig.Value;

        public void DoPatching()
        {
            harmony = new Harmony(Guid);
            harmony.PatchAll();
            if (LogLevel > ModdingUtils.LogLevel.None) 
                PluginLogger.LogInfo($"Patched.");
        }

        public void UnPatch()
        {
            harmony.UnpatchSelf();
            if (LogLevel > ModdingUtils.LogLevel.None) 
                PluginLogger.LogInfo($"Unpatched.");
        }

        public void DoConfig(ConfigFile Config)
        {
            LogLevelConfig = Config.Bind("Logging", "Log Level", ModdingUtils.LogLevel.Inherited);
            if (LogLevel > ModdingUtils.LogLevel.None) 
                PluginLogger.LogInfo($"Config Bound.");
        }

        private void Awake()
        {
            PluginLogger = Logger;
            DoConfig(Config);
            try
            {
                DoPatching();
                if (LogLevel > ModdingUtils.LogLevel.None)
                    PluginLogger.LogInfo($"Plugin is now Active.");
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                UnPatch();
                if (LogLevel > ModdingUtils.LogLevel.None)
                    PluginLogger.LogInfo($"Plugin failed to patch");
            }
        }
    }
}