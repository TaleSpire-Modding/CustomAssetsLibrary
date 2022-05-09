﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using LordAshes;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CustomAssetsLibrary.DTO
{
    public class AssetPackContent
    {
        public List<PlaceableData> Placeable = new List<PlaceableData>();
        public List<CreatureData> Creatures = new List<CreatureData>();
        public List<Atlas> Atlases = new List<Atlas>();
        public List<MusicData> Music = new List<MusicData>();

        internal BlobAssetReference<AssetPackIndex> GenerateBlobAssetReference()
        {
            var builder = new BlobBuilder(Allocator.Persistent);
            ref var blobAsset = ref builder.ConstructRoot<AssetPackIndex>();
            builder.Construct(ref blobAsset.Creatures, Creatures.Select(c => c.ToBRCreatureData(builder)).ToArray());
            builder.Construct(ref blobAsset.Placeables, Placeable.Select(c => c.ToBRPlaceableData(builder)).ToArray());
            builder.Construct(ref blobAsset.Atlases, Atlases.Select(c => c.ToBRAtlas(builder)).ToArray());
            builder.Construct(ref blobAsset.Music, Music.Select(c => c.ToBRMusic(builder)).ToArray());
            return builder.CreateBlobAssetReference<AssetPackIndex>(Allocator.Persistent);
        }

        public void FromJson(string path)
        {
            string text = File.ReadAllText(Path.Combine(path,"index.json"));
            var index = JsonConvert.DeserializeObject<CustomAssetsPlugin.Data.Index>(text);
            LoadFromIndex(index);
        }

        private Vector3 VectorFromString(string data)
        {
            var x = data.Split(',');
            return new Vector3(float.Parse(x[0]), float.Parse(x[1]), float.Parse(x[2]));
        }

        private quaternion RotationFromString(string data)
        {
            var x = data.Split(',');
            return new quaternion(float.Parse(x[0]), float.Parse(x[1]), float.Parse(x[2]),0f);
        }

        private void LoadFromIndex(CustomAssetsPlugin.Data.Index index)
        {
            var assetPackId = new NGuid(index.assetPackId);

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
                    Tags = tile.Tags,
                    IconAtlasIndex = tile.Icon.AtlasIndex,
                    IconAtlasRegion = tile.Icon.Region.ToRegion,
                    IsGmOnly = false,
                    Kind = PlaceableKind.Tile,
                    OrientationOffset = 0,
                    
                };
                foreach (var tileAsset in tile.Assets)
                {
                    newTile.Assets.Add(new AssetLoaderData
                    {
                        assetName = tileAsset.LoaderData.AssetName,
                        position = VectorFromString(tileAsset.Position),
                        scale = VectorFromString(tileAsset.Scale),
                        rotation = RotationFromString(tileAsset.Rotation),
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
                    Tags = prop.Tags,
                    IconAtlasIndex = prop.Icon.AtlasIndex,
                    IconAtlasRegion = prop.Icon.Region.ToRegion,
                    IsGmOnly = false,
                    Kind = PlaceableKind.Prop,
                    OrientationOffset = 0,
                };
                foreach (var propAsset in prop.Assets)
                {
                    newProp.Assets.Add(new AssetLoaderData
                    {
                        assetName = propAsset.LoaderData.AssetName,
                        position = VectorFromString(propAsset.Position),
                        scale = VectorFromString(propAsset.Scale),
                        rotation = RotationFromString(propAsset.Rotation),
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
                    baseRadius = creature.DefaultScale,
                    baseLoaderData = CreatureData.DefaultBase,
                    modelLoaderData = new AssetLoaderData
                    {
                        assetName = creature.MiniAsset.LoaderData.AssetName,
                        assetPackId = assetPackId,
                        position = VectorFromString(creature.MiniAsset.Position),
                        scale = VectorFromString(creature.MiniAsset.Scale),
                        path = creature.MiniAsset.LoaderData.BundleId,
                        rotation = RotationFromString(creature.MiniAsset.Rotation)
                    },
                    dbGroupTag = new DbGroupTag
                    {
                        Order = 0,
                        Name = creature.GroupTag
                    },
                    name = creature.Name,
                    description = creature.Name,
                    spellPos = new float3(0,0,0),
                    headPos = new float3(0, 0, 0),
                    hitPos = new float3(0, 0, 0),
                    torchPos = new float3(0, 0, 0),
                    baseCylinderBounds = new CreatureCylinderBounds(new float3(0,0,0), 1, 1),
                    creatureBounds = new Bounds(),
                    modelCylinderBounds = new CreatureCylinderBounds(new float3(0, 0, 0), 1, 1),
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