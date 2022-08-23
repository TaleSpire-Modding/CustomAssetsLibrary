using Bounce.TaleSpire.AssetManagement;
using Unity.Entities;
using UnityEngine;

namespace CustomAssetsKind.DTO
{
    struct CustomData
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
