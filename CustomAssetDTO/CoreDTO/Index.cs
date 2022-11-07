using System.Collections.Generic;
using BepInEx;
using Unity.Mathematics;
using UnityEngine;

namespace CustomAssetsCompiler.CoreDTO
{
    public partial class CustomAssetsPlugin : BaseUnityPlugin
    {
        public static partial class Data
        {
            public sealed class LoaderDataType
            {
                public string BundleId { get; set; } = "";
                public string AssetName { get; set; } = "";
            }

            public sealed class AssetType
            {
                public LoaderDataType LoaderData { get; set; } = default(LoaderDataType);
                public List<float> Position { get; set; } = new List<float>();
                public List<float> Rotation { get; set; } = new List<float>();
                public List<float> Scale { get; set; } = new List<float>();
            }

            public sealed class BoundsType
            {
                public List<float> m_Center { get; set; } = new List<float>();
                public List<float> m_Extent { get; set; } = new List<float>();

                public Bounds ToBounds()
                {
                    return new Bounds(AssetPackContent.VectorFromList(m_Center), AssetPackContent.VectorFromList(m_Extent));
                }
            }

            public sealed class RegionType
            {
                public string serializedVersion { get; set; } = "";
                public float x { get; set; } = 0f;
                public float y { get; set; } = 0f;
                public float width { get; set; } = 1f;
                public float height { get; set; } = 1f;

                public Rect ToRegion()
                {
                    return new Rect(x, y, width, height);
                }
            }

            public sealed class IconType
            {
                public int AtlasIndex { get; set; } = 0;
                public RegionType Region { get; set; } = default(RegionType);
            }

            public sealed class TileAndPropsType
            {
                public string Id { get; set; } = "";
                public string Name { get; set; } = "";
                public bool IsDeprecated { get; set; } = false;
                public string GroupTag { get; set; } = "";
                public List<string> Tags { get; set; } = new List<string>();
                public List<AssetType> Assets { get; set; } = new List<AssetType>();
                public bool IsInteractable { get; set; } = false;
                public BoundsType ColliderBoundsBound { get; set; } = default(BoundsType);
                public IconType Icon { get; set; } = default(IconType);
            }

            public sealed class CreatureType
            {
                public string Id { get; set; } = "";
                public string Name { get; set; } = "";
                public bool IsDeprecated { get; set; } = false;
                public string GroupTag { get; set; } = "";
                public List<string> Tags { get; set; } = new List<string>();
                public AssetType MiniAsset { get; set; } = default(AssetType);
                public AssetType BaseAsset { get; set; } = default(AssetType);
                public float DefaultScale { get; set; } = 0f;
                public IconType Icon { get; set; } = default(IconType);
                public float3 HeadPos { get; set; } = new float3(0,1,0);
                public float3 TorchPos { get; set; } = new float3(0, 1, 0);
                public float3 SpellPos { get; set; } = new float3(0, 1, 0);
                public float3 HitPos { get; set; } = new float3(0, 1, 0);
            }

            public sealed class MusicType
            {
                public string Id { get; set; } = "";
                public string Name { get; set; } = "";
                public bool IsDeprecated { get; set; } = false;
                public string GroupTag { get; set; } = "";
                public List<string> Tags { get; set; } = new List<string>();
                public LoaderDataType Assets { get; set; } = default(LoaderDataType);
            }

            public sealed class IconsAtlasesType
            {
                public string Path { get; set; } = "";
            }

            public sealed class CustomKinds
            {
                public string Kind;
                public string Catagory;
                public List<CustomKind> Entries = new List<CustomKind>();
            }

            public sealed class CustomKind
            {
                public string Id;
                public string Name;
                public string Description;
                public string Group;
                public string GroupTag;
                public List<string> Tags;
                public bool IsGmOnly;
                public bool IsDeprecated;
                public List<AssetType> Asset;
                public IconType Icon;
                public string OtherSerializedData;
            }

            public sealed class Index
            {
                public string assetPackId { get; set; } = "";
                public string Name { get; set; } = "Medieval Fantasy";
                public List<TileAndPropsType> Tiles { get; set; } = new List<TileAndPropsType>();
                public List<TileAndPropsType> Props { get; set; } = new List<TileAndPropsType>();
                public List<CreatureType> Creatures { get; set; } = new List<CreatureType>();
                public List<MusicType> Music { get; set; } = new List<MusicType>();
                public List<IconsAtlasesType> IconsAtlas { get; set; } = new List<IconsAtlasesType>();
                public List<CustomKinds> Custom = new List<CustomKinds>();
            }
        }
    }
}
