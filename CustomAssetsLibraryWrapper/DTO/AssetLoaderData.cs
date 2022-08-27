using System;
using Bounce.Unmanaged;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CustomAssetsLibrary.DTO
{
    [Serializable]
    public class AssetLoaderData
    {
        /// <summary>
        /// defaults to packaged asset pack
        /// </summary>
        public NGuid assetPackId;
        public string path = "";
        public string assetName = "";
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.one;

        public bool HasPath => !string.IsNullOrWhiteSpace(this.path);

        public static Bounce.TaleSpire.AssetManagement.AssetLoaderData CreateDummy() => new Bounce.TaleSpire.AssetManagement.AssetLoaderData()
        {
            path = "",
            assetName = "",
            position = Vector3.zero,
            rotation = Quaternion.identity,
            scale = Vector3.one
        };

        public void Pack(BlobBuilder builder, ref Bounce.TaleSpire.AssetManagement.AssetLoaderData.Packed packed)
        {
            packed.AssetPackId = assetPackId;

            builder.AllocateString(ref packed.BundleId, path);
            builder.AllocateString(ref packed.AssetId, assetName);

            packed.Position = (float3)position;
            packed.Rotation = (quaternion)rotation;
            packed.Scale = (float3)scale;
        }
    }
}
