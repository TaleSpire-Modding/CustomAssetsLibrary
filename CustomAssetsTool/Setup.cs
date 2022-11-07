using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LordAshes
{
    public static class Setup
    {
        public static Data.Index index = null;

        public static Data.LoaderDataType noBase = new Data.LoaderDataType() { BundleId = "", AssetName = "" };
        public static Data.LoaderDataType defaultBase = new Data.LoaderDataType() { BundleId = "char_base01_1462710208", AssetName = "clothBase" };

        /// <summary>
        /// Method used to register an asset with Custom Assets Library (CAL)
        /// </summary>
        /// <param name="info">AssetInfo file describing the asset to be registered</param>
        public static void RegisterAsset(Data.AssetInfo info)
        {
            Console.WriteLine("Custom Asset Plugin: Registering Asset '" + info.name + "' (" + info.location + ")");
            switch (info.kind.ToUpper())
            {
                case "AUDIO":
                case "AURA":
                case "CREATURE":
                case "EFFECT":
                case "FILTER":
                case "TRANSFORM":
                    index.Creatures.Add(new Data.CreatureType()
                    {
                        Id = CustomAssetsTool.GuidFromString(info.location).ToString(),
                        Name = info.name,
                        GroupTag = info.groupName,
                        Tags = (info.tags + ",Category:" + info.category + ",Kind:" + info.kind).Split(',').ToList<string>(),
                        IsDeprecated = info.isDeprecated,
                        BaseAsset = new Data.AssetType()
                        {
                            LoaderData = ((info.assetBase.ToUpper() == "NONE") ? noBase : (info.assetBase.ToUpper() == "DEFAULT") ? defaultBase : new Data.LoaderDataType() { AssetName = System.IO.Path.GetFileNameWithoutExtension(info.assetBase), BundleId = System.IO.Path.GetFileNameWithoutExtension(info.assetBase) }),
                            Scale = info.size + "," + info.size + "," + info.size
                        },
                        MiniAsset = new Data.AssetType()
                        {
                            LoaderData = new Data.LoaderDataType()
                            {
                                BundleId = info.location,
                                AssetName = System.IO.Path.GetFileNameWithoutExtension(info.location)
                            },
                            Scale = info.size + "," + info.size + "," + info.size
                        },
                        DefaultScale = info.size,
                        Icon = new Data.IconType()
                        {
                            AtlasIndex = 0,
                            Region = new Data.RegionType()
                            {
                                x = 0,
                                y = 0,
                                width = 1,
                                height = 1
                            }
                        }
                    });
                    break;
                case "MUSIC":
                    index.Music.Add(new Data.MusicType()
                    {
                        Id = CustomAssetsTool.GuidFromString(info.location).ToString(),
                        Name = info.name,
                        GroupTag = info.groupName,
                        Tags = (info.tags + ",[" + info.category + ":" + info.kind + "]").Split(',').ToList<string>(),
                        IsDeprecated = info.isDeprecated,
                        Assets = new Data.LoaderDataType()
                        {
                            BundleId = info.location,
                            AssetName = System.IO.Path.GetFileNameWithoutExtension(info.location)
                        }
                    });
                    break;
                case "PROP":
                    index.Props.Add(new Data.TileAndPropsType()
                    {
                        Id = CustomAssetsTool.GuidFromString(info.location).ToString(),
                        Name = info.name,
                        GroupTag = info.groupName,
                        Tags = (info.tags + ",[" + info.category + ":" + info.kind + "]").Split(',').ToList<string>(),
                        IsDeprecated = info.isDeprecated,
                        Assets = new List<Data.AssetType>()
                            {
                                new Data.AssetType()
                                {
                                    LoaderData = ((info.assetBase.ToUpper() == "NONE") ? noBase : (info.assetBase.ToUpper() == "DEFAULT") ? defaultBase : new Data.LoaderDataType() { AssetName = System.IO.Path.GetFileNameWithoutExtension(info.assetBase), BundleId = System.IO.Path.GetFileNameWithoutExtension(info.assetBase) }),
                                    Scale = info.size+","+info.size+","+info.size
                                }
                            },
                        IsInteractable = false,
                        ColliderBoundsBound = new Data.BoundsType()
                        {
                            m_Center = "0,0.5,0",
                            m_Extent = "0.5,0.5,0.5",
                        },
                        Icon = new Data.IconType()
                        {
                            AtlasIndex = 0,
                            Region = new Data.RegionType()
                            {
                                x = 0,
                                y = 0,
                                width = 1,
                                height = 1
                            }
                        }
                    });
                    break;
                case "TILE":
                    index.Tiles.Add(new Data.TileAndPropsType()
                    {
                        Id = CustomAssetsTool.GuidFromString(info.location).ToString(),
                        Name = info.name,
                        GroupTag = info.groupName,
                        Tags = (info.tags + ",[" + info.category + ":" + info.kind + "]").Split(',').ToList<string>(),
                        IsDeprecated = info.isDeprecated,
                        Assets = new List<Data.AssetType>()
                            {
                                new Data.AssetType()
                                {
                                    LoaderData = ((info.assetBase.ToUpper() == "NONE") ? noBase : (info.assetBase.ToUpper() == "DEFAULT") ? defaultBase : new Data.LoaderDataType() { AssetName = System.IO.Path.GetFileNameWithoutExtension(info.assetBase), BundleId = System.IO.Path.GetFileNameWithoutExtension(info.assetBase) }),
                                    Scale = info.size+","+info.size+","+info.size
                                }
                            },
                        IsInteractable = false,
                        ColliderBoundsBound = new Data.BoundsType()
                        {
                            m_Center = "0,0.5,0",
                            m_Extent = "0.5,0.5,0.5",
                        },
                        Icon = new Data.IconType()
                        {
                            AtlasIndex = 0,
                            Region = new Data.RegionType()
                            {
                                x = 0,
                                y = 0,
                                width = 1,
                                height = 1
                            }
                        }
                    });
                    break;
                case "SLAB":
                    break;
                case "ENCOUNTER":
                    break;
            }
        }

        /// <summary>
        /// Method used to generate an asset's portrait file
        /// </summary>
        /// <param name="info">AssetInfo file describing the asset to be registered</param>
        /// <param name="portrait">Texture2D to be converted to a portrait file</param>
        public static void CreatePortrait(Data.AssetInfo info, Texture2D portrait)
        {
            Console.WriteLine("Custom Asset Plugin: Creating Portrait File For Asset '" + info.name + "' (" + info.location + ")");
        }

        /// <summary>
        /// Method used to create an entry in the Asset Library for the asset
        /// </summary>
        /// <param name="info">AssetInfo file describing the asset to be registered</param>
        public static void CreateLibraryEntry(Data.AssetInfo info)
        {
            Console.WriteLine("Custom Asset Plugin: Creating Library Entry For Asset '" + info.name + "' (" + info.location + ")");
        }
    }
}

