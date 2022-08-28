using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using LordAshes;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CustomAssetsLibrary.DTO
{
    public class AssetPackContent
    {
        public string Name = "Medieval Fantasy";
        public List<PlaceableData> Placeable = new List<PlaceableData>();
        public List<CreatureData> Creatures = new List<CreatureData>();
        public List<Atlas> Atlases = new List<Atlas>();
        public List<MusicData> Music = new List<MusicData>();

        internal string assetPackString;

        internal BlobAssetReference<AssetPackIndex> GenerateBlobAssetReference()
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var blobAsset = ref builder.ConstructRoot<AssetPackIndex>();

            var placeables = builder.Allocate(ref blobAsset.Placeables, Placeable.Count);
            for (int i = 0; i < placeables.Length; i++)
            {
                Placeable[i].ToBRPlaceableData(builder, ref placeables[i]);
            }

            var creatures = builder.Allocate(ref blobAsset.Creatures, Creatures.Count);
            for (int i = 0; i < creatures.Length; i++)
            {
                Creatures[i].ToBRCreatureData(builder, ref creatures[i]);
            }

            var atlases = builder.Allocate(ref blobAsset.Atlases, Atlases.Count);
            for (int i = 0; i < atlases.Length; i++)
            {
                Atlases[i].ToBRAtlasData(builder, ref atlases[i]);
            }

            builder.Construct(ref blobAsset.Music, Music.Select(c => c.ToBRMusic(builder)).ToArray());

            builder.AllocateString(ref blobAsset.Name, Name);

            var api = builder.CreateBlobAssetReference<AssetPackIndex>(Allocator.Persistent);
            builder.Dispose();
            return api;
        }

        public void FromJson(string path)
        {
            var text = File.ReadAllText(Path.Combine(path,"index.json"));

            var index = SmartConvert.Json.DeserializeObject<CustomAssetsPlugin.Data.Index>(text);
            if (string.IsNullOrWhiteSpace(index.Name)) index.Name = "Medieval Fantasy";
            LoadFromIndex(index);
        }

        internal static float3 VectorFromList(List<float> data)
         => new float3(data.Count > 0 ? data[0] : 0, data.Count > 1 ? data[1] : 0, data.Count > 2 ? data[2] : 0);

        internal static quaternion RotationFromList(List<float> data)
         => new quaternion(data.Count > 0 ? data[0] : 0, data.Count > 1 ? data[1] : 0, data.Count > 2 ? data[2] : 0, data.Count > 3 ? data[3] : 0);

        private void LoadFromIndex(CustomAssetsPlugin.Data.Index index)
        {
            assetPackString = index.assetPackId;
            var assetPackId = new NGuid(index.assetPackId);
            Name = index.Name;
            Debug.Log($"asset pack being loaded:{assetPackId}");

            Debug.Log($"Loading Atlas packs");

            foreach (var atlas in index.IconsAtlas)
            {
                var newAtlas = new Atlas
                {
                    SizeX = 128,
                    SizeY = 128,
                    LocalPath = atlas.Path
                };
                Atlases.Add(newAtlas);
            }
            Debug.Log($"Loaded: {index.IconsAtlas.Count} Atlas packs");

            foreach (var music in index.Music)
            {
                Debug.Log($"Loaded {music.Name} into ");
                var newMusic = new MusicData
                {
                    Id = new NGuid(music.Id),
                    name = music.Name,
                    assetName = music.Assets.AssetName,
                    bundleId = music.Assets.BundleId,
                    assetPackId = assetPackId,
                    description = music.Name,
                    kind = Bounce.TaleSpire.AssetManagement.MusicData.MusicKind.Music,
                    tags = music.Tags,
                };
                Music.Add(newMusic);
                Debug.Log($"Loaded {music.Name} music into APC");
            }

            foreach (var tile in index.Tiles)
            {
                var newTile = new PlaceableData
                {
                    Id = new NGuid(tile.Id),
                    assetPackId = assetPackId,
                    IsDeprecated = tile.IsDeprecated,
                    Description = tile.Name,
                    Name = tile.Name,
                    Group = tile.GroupTag,
                    GroupTag = new DbGroupTag
                    {
                        Order = 0,
                        Name = tile.Tags.Count > 0 ? tile.Tags[0] : tile.GroupTag
                    },
                    Tags = tile.Tags,
                    IconAtlasIndex = tile.Icon.AtlasIndex,
                    IconAtlasRegion = tile.Icon.Region.ToRegion,
                    IsGmOnly = false,
                    Kind = PlaceableKind.Tile,
                    OrientationOffset = 0,
                    TotalVisualBounds = tile.ColliderBoundsBound.ToBounds(),
                    ColliderBoundsBound = tile.ColliderBoundsBound.ToBounds(),
                    Colliders = new List<Bounds>{ tile.ColliderBoundsBound.ToBounds() }
                };
                foreach (var tileAsset in tile.Assets)
                {
                    newTile.Assets.Add(new AssetLoaderData
                    {
                        assetName = tileAsset.LoaderData.AssetName,
                        position = VectorFromList(tileAsset.Position),
                        scale = VectorFromList(tileAsset.Scale),
                        rotation = RotationFromList(tileAsset.Rotation),
                        assetPackId = assetPackId,
                        path = tileAsset.LoaderData.BundleId
                    });
                }
                Placeable.Add(newTile);

                Debug.Log($"Loaded {tile.Name} tile into APC");
            }

            foreach (var prop in index.Props)
            {
                var newProp = new PlaceableData
                {
                    Id = new NGuid(prop.Id),
                    assetPackId = assetPackId,
                    IsDeprecated = prop.IsDeprecated,
                    Description = prop.Name,
                    Name = prop.Name,
                    Group = prop.GroupTag,
                    GroupTag = new DbGroupTag
                    {
                        Order = 0,
                        Name = prop.Tags.Count > 0 ? prop.Tags[0] : prop.GroupTag
                    },
                    Tags = prop.Tags,
                    IconAtlasIndex = prop.Icon.AtlasIndex,
                    IconAtlasRegion = prop.Icon.Region.ToRegion,
                    IsGmOnly = false,
                    Kind = PlaceableKind.Prop,
                    OrientationOffset = 0,
                    TotalVisualBounds = prop.ColliderBoundsBound.ToBounds(),
                    ColliderBoundsBound = prop.ColliderBoundsBound.ToBounds(),
                    Colliders = new List<Bounds> { prop.ColliderBoundsBound.ToBounds() },
                };
                foreach (var propAsset in prop.Assets)
                {
                    newProp.Assets.Add(new AssetLoaderData
                    {
                        assetName = propAsset.LoaderData.AssetName,
                        position = VectorFromList(propAsset.Position),
                        scale = VectorFromList(propAsset.Scale),
                        rotation = RotationFromList(propAsset.Rotation),
                        assetPackId = assetPackId,
                        path = propAsset.LoaderData.BundleId
                    });
                }
                Placeable.Add(newProp);
                Debug.Log($"Loaded {prop.Name} prop into APC");
            }

            foreach (var creature in index.Creatures)
            {
                var creatureData = new CreatureData
                {
                    id = new NGuid(creature.Id),
                    assetPackId = assetPackId,
                    defaultScale = creature.DefaultScale,
                    isDeprecated = creature.IsDeprecated,
                    isGmOnly = false,
                    group = creature.GroupTag,
                    baseRadius = 0.5f,
                    baseLoaderData = new AssetLoaderData
                    {
                        assetName = creature.BaseAsset.LoaderData.AssetName,
                        assetPackId = CreatureData.DefaultBase.assetPackId,
                        position = VectorFromList(creature.BaseAsset.Position),
                        scale = VectorFromList(creature.BaseAsset.Scale),
                        path = creature.BaseAsset.LoaderData.BundleId,
                        rotation = RotationFromList(creature.BaseAsset.Rotation)
                    },
                    modelLoaderData = new AssetLoaderData
                    {
                        assetName = creature.MiniAsset.LoaderData.AssetName,
                        assetPackId = assetPackId,
                        position = VectorFromList(creature.MiniAsset.Position),
                        scale = VectorFromList(creature.MiniAsset.Scale),
                        path = creature.MiniAsset.LoaderData.BundleId,
                        rotation = RotationFromList(creature.MiniAsset.Rotation)
                    },
                    dbGroupTag = new DbGroupTag
                    {
                        Order = 0,
                        Name = creature.Tags.Count > 0 ? creature.Tags[0] : creature.GroupTag
                    },
                    name = creature.Name,
                    description = creature.Name,
                    spellPos = float3.zero,
                    headPos = float3.zero,
                    hitPos = float3.zero,
                    torchPos = float3.zero,
                    baseCylinderBounds = new CreatureCylinderBounds(new float3(0,0,0), 1, 0.5f),
                    creatureBounds = new Bounds(),
                    modelCylinderBounds = new CreatureCylinderBounds(new float3(0, 0, 0), 1, 0.5f),
                    height = 1,
                    iconInfo = (creature.Icon.AtlasIndex,creature.Icon.Region.ToRegion),
                    tags = creature.Tags
                };
                Creatures.Add(creatureData);
                Debug.Log($"Loaded {creature.Name} creature into APC");
            }
        }
    }
}