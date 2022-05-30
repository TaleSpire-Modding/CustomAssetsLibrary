using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using Bounce.Singletons;
using Bounce.Unmanaged;
using HarmonyLib;
using MD5 = System.Security.Cryptography.MD5;
using UnityEngine;
using System.Reflection;
using Bounce.BlobAssets;
using Bounce.TaleSpire.AssetManagement;
using CustomAssetsLibrary.DTO;
using CustomAssetsLibrary.ReflecExt;
using Unity.Collections;
using Unity.Entities;
using CreatureData = Bounce.TaleSpire.AssetManagement.CreatureData;

// ReSharper disable once CheckNamespace
namespace ExtraAssetsLibrary.Patches
{
    [HarmonyPatch(typeof(AssetLoader), "Init")]
    public class AssetLoaderInit
    {
        public static void Prefix(IAssetContainer assetContainer, Transform parent, BlobView<Bounce.TaleSpire.AssetManagement.AssetLoaderData.Packed> data)
        {
            Debug.Log(data.Value.BundleId);
            Debug.Log(data.Value.Scale);
        }
    }

    [HarmonyPatch(typeof(AssetDb), "OnInstanceSetup")]
    public class AssetDbOnSetupInternalsPatch
    {
        

        private static string dirPlugin = BepInEx.Paths.PluginPath;

        public static bool HasSetup;

        /// <summary>
        /// Public for Testing only
        /// </summary>
        public static void Postfix(NativeHashMap<NGuid, BlobView<CreatureData>> ____creatures)
        {
           foreach (string directory in Directory.GetDirectories(dirPlugin))
                // foreach (string subDirectory in Directory.GetDirectories(directory))
                    LoadDirectory(directory);
           HasSetup = true;
        }

        /// <summary>
        /// Loads extra directory into Asset DB.
        /// This is public to allow other plugins to refresh post setup
        /// </summary>
        public static void LoadDirectory(string directory)
        {
            if (!File.Exists(Path.Combine(directory, "index.json"))) return; // Needs an index
            Debug.Log($"Index found in: {directory}");

            var instance = SimpleSingletonBehaviour<AssetLoadManager>.Instance;
            var filename = Path.GetFileName(directory);
            var guid = GenerateID(filename);
            // guid = new NGuid("d71427a1-5535-4fa7-82d7-4ca1e75edbfd");
            instance.call("RegisterAssetPack", new object[] { guid, directory });
            typeof(AssetDb)
                .GetMethod("LoadAssetPack", BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)
                .Invoke(null, new object[] { guid, directory });
            Debug.Log($"Pack {guid} Loaded");
        }

        /// <summary>
        /// Create an NGuid from a string via MD5 Hash
        /// </summary>
        /// <param name="id">text to get hashed</param>
        /// <returns>NGuid of the string</returns>
        internal static NGuid GenerateID(string id)
            => new NGuid(Guid.Parse(CreateMD5(id)));

        private static string CreateMD5(string input)
        {
            // Don't hash if can parse
            if (Guid.TryParse(input, out var result)) return input;
            
            // Use input string to calculate MD5 hash
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }
    }

}