using System.Collections.Generic;
using Bounce.TaleSpire.AssetManagement;
using Unity.Collections;
using Unity.Entities;

namespace CustomAssetsLibrary.DTO
{
    public class AssetPackContent
    {
        public List<PlaceableData> Props = new List<PlaceableData>();
        public List<PlaceableData> Tiles = new List<PlaceableData>();
        public List<CreatureData> Creatures = new List<CreatureData>();
        public List<Atlas> Atlases = new List<Atlas>();
        public List<MusicData> Music = new List<MusicData>();

        internal BlobAssetReference<AssetPackIndex> GenerateBlobAssetReference()
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var blobAsset = ref builder.ConstructRoot<AssetPackIndex>();

            var creatureArry = builder.Allocate(ref blobAsset.Creatures, Creatures.Count);
            for (int i = 0; i < Creatures.Count; i++)
            {
                creatureArry[i] = Creatures[i].ToBRCreatureData();
            }

            var placeableArry = builder.Allocate(ref blobAsset.Placeables, Props.Count + Tiles.Count);
            for (int i = 0; i < Props.Count; i++)
            {
                placeableArry[i] = Props[i].ToBRPlaceableData();
            }
            for (int i = 0; i < Tiles.Count; i++)
            {
                placeableArry[i+Props.Count] = Tiles[i].ToBRPlaceableData();
            }

            var atlasArry = builder.Allocate(ref blobAsset.Atlases, Atlases.Count);
            for (int i = 0; i < Atlases.Count; i++)
            {
                atlasArry[i] = Atlases[i].ToBRAtlas();
            }

            var musicArry = builder.Allocate(ref blobAsset.Music, Music.Count);
            for (int i = 0; i < Music.Count; i++)
            {
                musicArry[i] = Music[i].ToBRMusic();
            }

            var blobref = builder.CreateBlobAssetReference<AssetPackIndex>(Allocator.Persistent);

            // var indexDestinationLocation = Path.Combine(assetBundleDirectory, "index");
            // var writer = new StreamBinaryWriter(indexDestinationLocation);
            // writer.Write(blobref);
            return blobref;
        }
    }
}