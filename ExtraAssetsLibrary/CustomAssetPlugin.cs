using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Bounce.Unmanaged;
using CustomAssetsLibrary.DTO;
using ExtraAssetsLibrary.Patches;
using HarmonyLib;
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
    public class CustomAssetPlugin : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.PM.plugins.CustomAssetLib";
        public const string Version = "1.0.0.0";
        private const string Name = "Plugin Masters' Custom Asset Library";

        internal static ConfigEntry<bool> AutoClear { get; set; }
        internal static ConfigEntry<LogLevel> LogLevel { get; set; }

        /// <summary>
        ///     List of all callbacks being run on an asset being loaded
        /// </summary>
        public static DictionaryList<string, Func<NGuid, AssetDb.DbEntry.EntryKind, bool>> CoreAssetPrefixCallbacks =
            new DictionaryList<string, Func<NGuid, AssetDb.DbEntry.EntryKind, bool>>();

        public static void DoPatching()
        {
            var harmony = new Harmony(Guid);
            harmony.PatchAll();
            if (LogLevel.Value > CustomAssetsLibrary.LogLevel.None) Debug.Log($"Extra Asset Library Plugin: Patched.");
        }

        public static void DoConfig(ConfigFile Config)
        {
            AutoClear = Config.Bind("Mini Loading", "Auto Clear Failed Minis", false);
            LogLevel = Config.Bind("Logging", "Level", CustomAssetsLibrary.LogLevel.Low);
            if (LogLevel.Value > CustomAssetsLibrary.LogLevel.None) Debug.Log($"Extra Asset Library Plugin: Config Bound.");
        }

        private void Awake()
        {
            DoConfig(Config);
            DoPatching();
            if (LogLevel.Value > CustomAssetsLibrary.LogLevel.None) Debug.Log($"Extra Asset Library Plugin:{Name} is Active.");
        }

        public static void WritePack(string directory, AssetPackContent content)
        { 
            var blobref = content.GenerateBlobAssetReference(); 
            var indexDestinationLocation = Path.Combine(directory, "index");
            var writer = new StreamBinaryWriter(indexDestinationLocation);
            writer.Write(blobref);
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