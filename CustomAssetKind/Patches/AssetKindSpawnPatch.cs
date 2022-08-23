using System;
using System.Collections.Generic;
using System.Linq;
using Bounce.BlobAssets;
using Bounce.Singletons;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using CustomAssetsKind.DTO;
using CustomAssetsLoader;
using GameSequencer;
using HarmonyLib;
using Unity.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace CustomAssetsLibrary.Patches
{
    [HarmonyPatch(typeof(AssetSpawnFromTool), "SpawnUsingTool")]
    public class AssetSpawnFromToolSpawnUsingToolPatch
    {
        internal static Dictionary<string, Action<CustomData>> SpawnTool = new Dictionary<string, Action<CustomData>>();
        internal static Dictionary<string, NativeHashMap<BoardAssetGuid, BlobView<CustomData>>> AssetDb = new Dictionary<string, NativeHashMap<BoardAssetGuid, BlobView<CustomData>>>();

        public static void Postfix(ref NGuid nGuid)
        {
            if (!AssetSpawnFromToolCheckPatch.assetFound)
            {
                var boardAssetGuid = new BoardAssetGuid(nGuid);
                var found = false;

                foreach (var entry in AssetDb.AsParallel())
                {
                    if (found || !entry.Value.TryGetValue(boardAssetGuid, out var data)) continue;
                    found = true;
                    if (data.IsCreated)
                        SpawnTool[entry.Key](data.Value);
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

        public static void Postfix(ref NGuid nGuid, ref bool __result)
        {
            if (!assetFound == true)
                assetFound = __result;
        }
    }
}