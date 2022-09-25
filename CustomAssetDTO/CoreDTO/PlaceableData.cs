using System.Collections.Generic;
using Bounce.BlobAssets;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using Spaghet.Compiler;
using Spaghet.Runtime;
using Unity.Entities;
using UnityEngine;

namespace CustomAssetsCompiler.CoreDTO
{
    public sealed class PlaceableData
    {
        public NGuid assetPackId;
        public NGuid Id;
        public string Name = "";
        public string Description = "";
        public string Group = "";
        public DbGroupTag GroupTag;
        public List<string> Tags = new List<string>();
        public PlaceableKind Kind = PlaceableKind.Tile;
        public bool IsGmOnly;
        public bool IsDeprecated;
        public int OrientationOffset;
        public List<AssetLoaderData> Assets = new List<AssetLoaderData>();
        public List<Bounds> Colliders = new List<Bounds>();
        public Bounds ColliderBoundsBound;
        public Bounds TotalVisualBounds;
        public int IconAtlasIndex;
        public Rect IconAtlasRegion;

        internal void ToBRPlaceableData(BlobBuilder builder, ref Bounce.TaleSpire.AssetManagement.PlaceableData placeable)
        {
            placeable.OrientationOffset = OrientationOffset;
            placeable.Id = new BoardAssetGuid(Id);
            placeable.IsGmOnly = IsGmOnly;
            placeable.IsDeprecated = IsDeprecated;
            builder.AllocateString(ref placeable.Name, Name);
            builder.AllocateString(ref placeable.Description, Description);
            builder.AllocateString(ref placeable.Group, Group);
            placeable.TotalVisualBounds = TotalVisualBounds;
            placeable.ColliderBoundsBound = ColliderBoundsBound;
            placeable.ColliderIndex = new PlaceableCollidersIndex();
            
            var colliderArray = builder.Allocate<Bounds>(ref placeable.Colliders, Colliders.Count);
            for (int i = 0; i < Colliders.Count; i++)
            {
                colliderArray[i] = Colliders[i];
            }

            if (Tags == null)
                placeable.Tags = new BlobArray<BlobString>();
            else
                builder.ConstructStringArray(ref placeable.Tags, Tags.ToArray());

            var blobBuilderArray = builder.Allocate(ref placeable.Assets, Assets.Count);
            for (int i = 0; i < Assets.Count; i++)
            {
                Assets[i].Pack(builder, ref blobBuilderArray[i]);
            }

            builder.Allocate<AssetScriptIndex>(ref placeable.AssetScripts, 0);
            ConstructEmptyScript(builder, ref placeable, false);
            placeable.IconAtlasIndex = IconAtlasIndex;
            placeable.IconAtlasRegion = IconAtlasRegion;
            placeable.Kind = Kind;

            ref var local1 = ref builder.Allocate(ref placeable.GroupTag);
            builder.AllocateString(ref local1.Name, GroupTag.Name);
            local1.Order = GroupTag.Order;
        }

        private static void ConstructEmptyScript(
            BlobBuilder builder,
            ref Bounce.TaleSpire.AssetManagement.PlaceableData placeable,
            bool anyAssetsHaveScripts)
        {
            ref Spaghet.Runtime.StateMachineScript local = ref builder.Allocate<Spaghet.Runtime.StateMachineScript>(ref placeable.StateMachineScript);
            if (anyAssetsHaveScripts)
                CompileResults.ConstructRunAll(builder, ref local.Compiled);
            else
                CompileResults.ConstructEmpty(builder, ref local.Compiled);
            builder.Allocate<Menu.Packed>(ref local.Menus, 0);
        }
    }
}
