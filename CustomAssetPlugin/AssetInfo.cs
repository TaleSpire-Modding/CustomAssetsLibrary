using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PluginMasters
{
    public partial class CustomAssetsLibraryPlugin : BaseUnityPlugin
    {
        public static partial class Data
        {
            public class SlabInfo
            {
                public Unity.Mathematics.float3 position { get; set; }
                public string code { get; set; } = "";

            }

            public class Locations
            {
                public readonly string root = "0.0,0.0,0.0,0.0,0.0,0.0";
                public string head { get; set; } = "0.0,0.7,0.0,0.0,0.0,0.0";
                public string hit { get; set; } = "0.0,0.5,0.0,0.0,0.0,0.0";
                public string spell { get; set; } = "0.0,0.5,0.0,0.0,0.0,0.0";
                public string torch { get; set; } = "0.0,0.5,0.0,0.0,0.0,0.0";
                public string handRight { get; set; } = "0.3,1.25,0.0,0.0,0.0,0.0";
                public string handLeft { get; set; } = "-0.3,1.25,0.0,0.0,0.0,0.0";
            }

            public class Collider
            {
                public string center { get; set; } = "0.5,0.5,0.5";
                public string extent { get; set; } = "0.5,0.5,0.5";
            }

            public class MeshAdjustments
            {
                public string size { get; set; } = "1.0,1.0,1.0";
                public string rotationOffset { get; set; } = "0.0,0.0,0.0";
                public string positionOffset { get; set; } = "0.0,0.0,0.0";
            }

            public class LinkRequest
            {
                public string key { get; set; } = "";
                public string value { get; set; } = "";
                public bool legacy { get; set; } = true;
            }

            public class AssetInfo
            {
                public string id { get; set; } = "";
                public string name { get; set; } = "";
                public string prefab { get; set; } = "";
                public string kind { get; set; } = "Creature";
                public string category { get; set; } = "Creature";
                public string header { get; set; } = "Medieval Fantasy";
                public string groupName { get; set; } = "Custom Content";
                public string description { get; set; } = "";
                public string tags { get; set; } = "";
                public string author { get; set; } = "Anonymous";
                public string version { get; set; } = "Unversioned";
                public string comment { get; set; } = "";
                public string[] variants { get; set; } = null;
                public string chainLoad { get; set; } = null;
                public string anchor { get; set; } = "root";
                public float timeToLive { get; set; } = 0f;
                public float size { get; set; } = 1.0f;
                public string code { get; set; } = "";
                public string location { get; set; } = "";
                public string assetBase { get; set; } = "DEFAULT";
                public MeshAdjustments mesh { get; set; } = new MeshAdjustments();
                public Collider collider { get; set; } = new Collider();
                public Locations locations { get; set; } = new Locations();
                public LinkRequest[] linkRequests { get; set; } = new LinkRequest[] { };
                public bool isDeprecated { get; set; } = false;

                public Data.AssetInfo Clone()
                {
                    return new AssetInfo()
                    {
                        id = this.id,
                        name = this.name,
                        prefab = this.prefab,
                        kind = this.kind,
                        category = this.category,
                        header = this.header,
                        groupName = this.groupName,
                        description = this.description,
                        tags = this.tags,
                        variants = this.variants,
                        chainLoad = this.chainLoad,
                        anchor = this.anchor,
                        author = this.author,
                        version = this.version,
                        comment = this.comment,
                        timeToLive = this.timeToLive,
                        size = this.size,
                        code = this.code,
                        location = this.location,
                        assetBase = this.assetBase,
                        mesh = this.mesh,
                        collider = this.collider,
                        locations = this.locations,
                        linkRequests = this.linkRequests,
                        isDeprecated = this.isDeprecated
                    };
                }
            }
        }
    }
}
