using System.IO;
using CustomAssetsLibrary;
using HarmonyLib;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace ExtraAssetsLibrary.Patches
{
    [HarmonyPatch(typeof(Path), "Combine", typeof(string),typeof(string))]
    public class PathCombine
    {
        
        public static bool Prefix(ref string path1, ref string path2, ref string __result)
        {
            bool proceed = true;
            if (CustomAssetLib.LogLevel.Value >= LogLevel.Medium)
                Debug.Log($"Before: {path1}, {path2}");

            if (path1 == "Assets" && path2.StartsWith("../"))
            {
                __result = path2.Remove(3);
                proceed = false;
            }
            else
            {
                while (path2.StartsWith("../"))
                {
                    path2 = path2.Remove(3);
                    path1 = Directory.GetParent(path1).FullName;
                }
            }

            if (CustomAssetLib.LogLevel.Value >= LogLevel.Medium)
                Debug.Log($"After: {path1}, {path2}");

            return proceed;
        }

    }

}