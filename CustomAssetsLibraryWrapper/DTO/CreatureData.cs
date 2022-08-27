using System.Collections.Generic;
using Bounce.BlobAssets;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CustomAssetsLibrary.DTO
{
    public class CreatureData
    {
        // Base size is 0 but still there for CMP
        public static readonly AssetLoaderData NoBase = new AssetLoaderData
        {
            assetPackId = new NGuid("d71427a1-5535-4fa7-82d7-4ca1e75edbfd"),
            path = "char_base01_1462710208",
            assetName = "clothBase",
            position = new Vector3(0f, 0f, 0f),
            scale = new Vector3(0f, 0f, 0f),
            rotation = new Quaternion(0f, -0.5735762f, 0f, 0.8191524f)
        };

        // Default data for base
        public static readonly AssetLoaderData DefaultBase = new AssetLoaderData
        {
            assetPackId = new NGuid("d71427a1-5535-4fa7-82d7-4ca1e75edbfd"),
            path = "char_base01_1462710208",
            assetName = "clothBase",
            position = new Vector3(0f, 0f, 0f),
            scale = new Vector3(0.6f, 0.6f, 0.6f),
            rotation = new Quaternion(0f, -0.5735762f, 0f, 0.8191524f)
        };

        public NGuid assetPackId;
        public NGuid id;
        public bool isGmOnly;
        public bool isDeprecated;
        public string name = "";
        public string description = "";
        public string group = "";
        public DbGroupTag dbGroupTag;
        public List<string> tags = new List<string>();
        public AssetLoaderData baseLoaderData = DefaultBase;
        public AssetLoaderData modelLoaderData;
        public CreatureCylinderBounds baseCylinderBounds;
        public CreatureCylinderBounds modelCylinderBounds;
        public float3 headPos;
        public float3 torchPos;
        public float3 spellPos;
        public float3 hitPos;
        public float baseRadius;
        public float height;
        public float defaultScale;
        public Bounds creatureBounds;
        public (int, Rect) iconInfo;

        internal void ToBRCreatureData(BlobBuilder builder, ref Bounce.TaleSpire.AssetManagement.CreatureData output)
        {
            Construct(builder, ref output, assetPackId, id, isGmOnly, isDeprecated, name, description, group, dbGroupTag, tags.ToArray(), baseLoaderData, modelLoaderData, baseCylinderBounds, modelCylinderBounds, headPos, torchPos, spellPos, hitPos, baseRadius, height, defaultScale, creatureBounds, iconInfo);
        }

        internal static void Construct(
      BlobBuilder builder,
      ref Bounce.TaleSpire.AssetManagement.CreatureData creature,
      NGuid assetPackId,
      NGuid id,
      bool isGmOnly,
      bool isDeprecated,
      string name,
      string description,
      string group,
      DbGroupTag dbGroupTag,
      string[] tags,
      AssetLoaderData baseLoaderData,
      AssetLoaderData modelLoaderData,
      CreatureCylinderBounds baseCylinderBounds,
      CreatureCylinderBounds modelCylinderBounds,
      float3 headPos,
      float3 torchPos,
      float3 spellPos,
      float3 hitPos,
      float baseRadius,
      float height,
      float defaultScale,
      Bounds creatureBounds,
      (int, Rect) iconInfo)
        {
            creature.Id = new BoardAssetGuid(id);
            builder.AllocateString(ref creature.Name, name);
            builder.AllocateString(ref creature.Description, description);
            builder.AllocateString(ref creature.Group, group);

            ref var local1 = ref builder.Allocate(ref creature.GroupTag);
            builder.AllocateString(ref local1.Name, dbGroupTag.Name);
            local1.Order = dbGroupTag.Order;

            if (tags == null)
                creature.Tags = new BlobArray<BlobString>();
            else
                builder.ConstructStringArray(ref creature.Tags, tags);

            creature.IsGmOnly = isGmOnly;
            creature.IsDeprecated = isDeprecated;

            if (baseLoaderData != null)
            {
                ref Bounce.TaleSpire.AssetManagement.AssetLoaderData.Packed local2 = ref builder.Allocate(ref creature.BaseAsset);
                baseLoaderData.Pack(builder, ref local2);
            }
            ref Bounce.TaleSpire.AssetManagement.AssetLoaderData.Packed local3 = ref builder.Allocate(ref creature.ModelAsset);
            modelLoaderData.Pack(builder, ref local3);

            creature.HeadPos = headPos;
            creature.TorchPos = torchPos;
            creature.SpellPos = spellPos;
            creature.HitPos = hitPos;
            
            creature.BaseRadius = baseRadius;
            creature.Height = height;
            creature.DefaultScale = defaultScale;

            creature.BaseCylinderBounds = baseCylinderBounds;
            creature.ModelCylinderBounds = modelCylinderBounds;

            creature.CreatureBounds = creatureBounds;
            creature.IconAtlasIndex = iconInfo.Item1;
            creature.IconAtlasRegion= iconInfo.Item2;
        }
    }
}
