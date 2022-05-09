using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bounce.BlobAssets;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using HarmonyLib;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

namespace CustomAssetsLibrary.Patches
{
    [HarmonyPatch(typeof(AssetDb), "LoadAssetPack")]
    public class AssetDbLoadPatch
    { 
        public static void Prefix(NGuid assetPackId, string packDir)
        {
            Debug.Log($"Loading {assetPackId.ToHexString()} from {packDir}");
            string str = Path.Combine(packDir, "index");
            var reader = new StreamBinaryReader(str);
            BlobAssetReference<AssetPackIndex> bref = reader.Read<AssetPackIndex>();
            reader.Dispose();

            Debug.Log($"Found: {bref.Value.Creatures.Length} Creatures");
            Debug.Log($"Found: {bref.Value.Music.Length} Music");
            Debug.Log($"Found: {bref.Value.Placeables.Length} Placeables");
            Debug.Log($"Found: {bref.Value.Atlases.Length} Atlases");

            /*
            foreach (var creature in bref.Value.Creatures.ToArray())
            {
                Debug.Log(creature.Name.ToString());
            }*/
        }
    }

    [HarmonyPatch(typeof(AssetLoader), "Init")]
    public class AssetLoaderInit
    {
        public static void Prefix(IAssetContainer assetContainer, Transform parent, BlobView<AssetLoaderData.Packed> data)
        {
            Debug.Log(data.Value);
        }
    }
}
