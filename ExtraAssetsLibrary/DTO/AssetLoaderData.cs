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
        public NGuid? assetPackId;
        public string path = "";
        public string assetName = "";
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public bool HasPath => !string.IsNullOrWhiteSpace(this.path);

        public static Bounce.TaleSpire.AssetManagement.AssetLoaderData CreateDummy() => new Bounce.TaleSpire.AssetManagement.AssetLoaderData()
        {
            path = "",
            assetName = "",
            position = Vector3.zero,
            rotation = Quaternion.identity,
            scale = Vector3.one
        };

        public void Pack(BlobBuilder builder, NGuid assetPackId, ref Bounce.TaleSpire.AssetManagement.AssetLoaderData.Packed packed)
        {
            packed.AssetPackId = this.assetPackId ?? assetPackId;
            builder.AllocateString(ref packed.BundleId, this.path);
            builder.AllocateString(ref packed.AssetId, this.assetName);
            packed.Position = (float3)this.position;
            packed.Rotation = (quaternion)this.rotation;
            packed.Scale = (float3)this.scale;
        }
    }
}
