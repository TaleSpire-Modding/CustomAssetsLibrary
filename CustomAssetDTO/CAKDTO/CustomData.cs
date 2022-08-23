using System.Collections.Generic;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using Unity.Entities;
using UnityEngine;
using AssetLoaderData = CustomAssetsCompiler.CoreDTO.AssetLoaderData;

namespace Bounce.TaleSpire.AssetManagement
{
    public struct CustomKinds
    {
        public BlobString Kind;
        public BlobString Catagory;
        public BlobArray<CustomData> Entries;
    }

    public struct CustomData
    {
        public BoardAssetGuid Id;
        public BlobString Name;
        public BlobString Description;
        public BlobString Group;
        public BlobPtr<DbGroupTag.Packed> GroupTag;
        public BlobArray<BlobString> Tags;
        public bool IsGmOnly;
        public bool IsDeprecated;
        public BlobPtr<AssetLoaderData.Packed> ModelAsset;
        public int IconAtlasIndex;
        public Rect IconAtlasRegion;
        public BlobString OtherSerializedData;
    }
}

namespace CustomAssetsKind.DTO
{
    public class CustomKinds
    {
        public string Kind;
        public string Catagory;
        public List<CustomData> Entries = new List<CustomData>();
    }

    public class CustomData
    {
        public NGuid Id;
        public string Name;
        public string Description;
        public string Group;
        public DbGroupTag GroupTag;
        public List<string> Tags;
        public bool IsGmOnly;
        public bool IsDeprecated;
        public AssetLoaderData ModelAsset;
        public (int, Rect) iconInfo;
        public string OtherSerializedData;

        public void ToBRCustomData(BlobBuilder builder, ref Bounce.TaleSpire.AssetManagement.CustomData output)
        {

        }
    }
}