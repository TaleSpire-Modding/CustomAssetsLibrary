using System.Collections.Generic;
using Bounce.Unmanaged;
using Unity.Collections;
using Unity.Entities;

namespace CustomAssetsLibrary.DTO
{
    public class MusicData
    {
        public NGuid assetPackId;
        public NGuid Id;
        public string name = "";
        public string description = "";
        public List<string> tags = new List<string>();
        public string bundleId = "";
        public string assetName = "";
        public Bounce.TaleSpire.AssetManagement.MusicData.MusicKind kind = Bounce.TaleSpire.AssetManagement.MusicData.MusicKind.Music;

        internal Bounce.TaleSpire.AssetManagement.MusicData ToBRMusic(BlobBuilder builder)
        {
            ref var blobAsset = ref builder.ConstructRoot<Bounce.TaleSpire.AssetManagement.MusicData>();
            Bounce.TaleSpire.AssetManagement.MusicData.Construct(builder,ref blobAsset,assetPackId,Id,name,description,tags.ToArray(),bundleId,assetName,kind);
            return blobAsset;
        }
    }
}
