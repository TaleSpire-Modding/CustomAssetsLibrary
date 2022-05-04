using System.Collections.Generic;
using Bounce.BlobAssets;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using Unity.Collections;
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
            scale = new Vector3(1f, 1f, 1f),
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

        internal Bounce.TaleSpire.AssetManagement.CreatureData ToBRCreatureData()
        {
            var builder = new BlobBuilder(Allocator.Persistent);
            ref var output = ref builder.ConstructRoot<Bounce.TaleSpire.AssetManagement.CreatureData>();
            Construct(builder, ref output, assetPackId, id, isGmOnly, isDeprecated, name, description, group, dbGroupTag, tags.ToArray(), baseLoaderData, modelLoaderData, baseCylinderBounds, modelCylinderBounds, headPos, torchPos, spellPos, hitPos, baseRadius, height, defaultScale, creatureBounds, iconInfo);
            return output;
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
            int i = 0;
            Debug.Log($"Line{i++}");
            creature.Id = id;
            creature.IsGmOnly = isGmOnly;
            Debug.Log($"Line{i++}");
            creature.IsDeprecated = isDeprecated;
            builder.AllocateString(ref creature.Name, name);
            Debug.Log($"Line{i++}");
            builder.AllocateString(ref creature.Description, description);
            builder.AllocateString(ref creature.Group, group);
            Debug.Log($"Line{i++}");
            // ref DbGroupTag.Packed local1 = ref builder.Allocate(ref creature.GroupTag);
            // dbGroupTag.Pack(builder, ref local1);
            Debug.Log($"Line{i++}");
            if (tags == null)
                creature.Tags = new BlobArray<BlobString>();
            else
                builder.ConstructStringArray(ref creature.Tags, tags);
            Debug.Log($"Line{i++}");
            if (baseLoaderData != null)
            {
                ref Bounce.TaleSpire.AssetManagement.AssetLoaderData.Packed local2 = ref builder.Allocate(ref creature.BaseAsset);
                baseLoaderData.Pack(builder, baseLoaderData.assetPackId ?? assetPackId, ref local2);
            }
            Debug.Log($"Line{i++}");
            ref Bounce.TaleSpire.AssetManagement.AssetLoaderData.Packed local3 = ref builder.Allocate(ref creature.ModelAsset);
            modelLoaderData.Pack(builder, modelLoaderData.assetPackId ?? assetPackId, ref local3);
            Debug.Log($"Line{i++}");
            creature.HeadPos = headPos;
            creature.TorchPos = torchPos;
            Debug.Log($"Line{i++}");
            creature.SpellPos = spellPos;
            creature.BaseRadius = baseRadius;
            Debug.Log($"Line{i++}");
            creature.HitPos = hitPos;
            creature.Height = height;
            Debug.Log($"Line{i++}");
            creature.DefaultScale = defaultScale;
            creature.CreatureBounds = creatureBounds;
            Debug.Log($"Line{i++}");
            creature.ModelCylinderBounds = modelCylinderBounds;
            creature.BaseCylinderBounds = baseCylinderBounds;
            Debug.Log($"Line{i++}");
            ref int local4 = ref creature.IconAtlasIndex;
            ref Rect local5 = ref creature.IconAtlasRegion;
            Debug.Log($"Line{i++}");
            (int, Rect) tuple = iconInfo;
            int num = tuple.Item1;
            Debug.Log($"Line{i++}");
            local4 = num;
            local5 = tuple.Item2;
        }
    }
}
