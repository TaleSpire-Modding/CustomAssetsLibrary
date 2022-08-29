using System.Linq;
using Bounce.Unmanaged;
using CustomAssetsKind.Singleton;
using HarmonyLib;


// ReSharper disable once CheckNamespace
namespace CustomAssetsLibrary.Patches
{
    [HarmonyPatch(typeof(AssetSpawnFromTool), "SpawnUsingTool")]
    public class AssetSpawnFromToolSpawnUsingToolPatch
    {
        public static void Postfix(ref NGuid nGuid)
        {
            if (!AssetSpawnFromToolCheckPatch.assetFound)
            {
                var boardAssetGuid = new BoardAssetGuid(nGuid);
                var found = false;

                foreach (var entry in CustomAssetDb.CustomAssets.AsParallel())
                {
                    if (found || !entry.Value.TryGetValue(boardAssetGuid, out var data) && data.IsCreated) continue;
                    found = true;
                    CustomAssetDb.SpawnTool[entry.Key](data.Value);
                }
            }
            AssetSpawnFromToolCheckPatch.assetFound = false;
        }
    }

    [HarmonyPatch(typeof(AssetSpawnFromTool), "CheckIfAssetCreature")]
    [HarmonyPatch(typeof(AssetSpawnFromTool), "CheckIfAssetProp")]
    [HarmonyPatch(typeof(AssetSpawnFromTool), "CheckIfAssetTile")]
    [HarmonyPatch(typeof(AssetSpawnFromTool), "CheckIfEmote")]
    public class AssetSpawnFromToolCheckPatch
    {
        public static bool assetFound;

        public static void Postfix(ref NGuid nGuid, ref bool __result) => assetFound |= __result;
    }
}