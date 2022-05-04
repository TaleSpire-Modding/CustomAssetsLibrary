using System.Collections.Generic;
using BepInEx;
using UnityEngine;

namespace CustomAssetsLibrary.DTO
{
    public partial class CustomAssetsPlugin : BaseUnityPlugin
    {
        public static partial class Data
        {
            public class LoaderDataType
            {
                public string BundleId { get; set; } = "";
                public string AssetName { get; set; } = "";
            }

            public class AssetType
            {
                public LoaderDataType LoaderData { get; set; } = default(LoaderDataType);
                public string Position { get; set; } = "0,0,0";
                public string Rotation { get; set; } = "0,0,0,0";
                public string Scale { get; set; } = "1,1,1";
            }

            public class BoundsType
            {
                public string m_Center { get; set; } = "0,0,0";
                public string m_Extent { get; set; } = "0,0,0";
            }

            public class RegionType
            {
                public string serializedVersion { get; set; } = "";
                public float x { get; set; } = 0f;
                public float y { get; set; } = 0f;
                public float width { get; set; } = 0f;
                public float height { get; set; } = 0f;

                public Rect ToRegion => new Rect(x, y, width, height);
            }

            public class IconType
            {
                public int AtlasIndex { get; set; } = 0;
                public RegionType Region { get; set; } = default(RegionType);
            }

            public class TileAndPropsType
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

            public class CreatureType
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
            }

            public class MusicType
            {
                public string Id { get; set; } = "";
                public string Name { get; set; } = "";
                public bool IsDeprecated { get; set; } = false;
                public string GroupTag { get; set; } = "";
                public List<string> Tags { get; set; } = new List<string>();
                public LoaderDataType Assets { get; set; } = default(LoaderDataType);
            }

            public class IconsAtlasesType
            {
                public string Path { get; set; } = "";
            }

            public class Index
            {
                public string assetPackId { get; set; } = "";
                public List<TileAndPropsType> Tiles { get; set; } = new List<TileAndPropsType>();
                public List<TileAndPropsType> Props { get; set; } = new List<TileAndPropsType>();
                public List<CreatureType> Creatures { get; set; } = new List<CreatureType>();
                public List<MusicType> Music { get; set; } = new List<MusicType>();
                public List<IconsAtlasesType> IconsAtlas { get; set; } = new List<IconsAtlasesType>();
            }
        }
    }
}
