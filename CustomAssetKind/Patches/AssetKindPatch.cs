using Bounce.Unmanaged;
using CustomAssetsLoader;
using HarmonyLib;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace CustomAssetsLibrary.Patches
{
    [HarmonyPatch(typeof(AssetLoadManager), "DispatchAtlasLoads")]
    public class UIAssetBrowserPatch
    {
        public static void Postfix(ref Texture2D[] __result)
        {
            if (CustomAssetKindPlugin.LogLevel.Value >= LogLevel.High)
                Debug.Log($"Atlas Indexes Found: {__result.Length}");
        }
    }
    

    [HarmonyPatch(typeof(AssetLoadManager), "LoadInternalAssetPack")]
    public class AssetDbLoadInternalAssetPackPatch
    {
        public static void Prefix(ref NGuid assetPackId, ref string packDir)
        {
            if (CustomAssetKindPlugin.LogLevel.Value >= LogLevel.High)
                Debug.Log($"Started loading in {assetPackId} from {packDir}");
        }

        public static void Postfix(ref NGuid assetPackId, ref string packDir)
        {
            if (CustomAssetKindPlugin.LogLevel.Value >= LogLevel.High)
                Debug.Log($"Loaded in {assetPackId} from {packDir}");
        }
    }
}