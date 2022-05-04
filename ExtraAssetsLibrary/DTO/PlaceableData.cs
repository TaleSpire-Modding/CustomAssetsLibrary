using System.Collections.Generic;
using Bounce.BlobAssets;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using Spaghet.Compiler;
using Spaghet.Runtime;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CustomAssetsLibrary.DTO
{
    public class PlaceableData
    {
        public NGuid assetPackId;
        public NGuid Id;
        public string Name = "";
        public string Description = "";
        public string Group = "";
        public DbGroupTag.Packed GroupTag;
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

        internal Bounce.TaleSpire.AssetManagement.PlaceableData ToBRPlaceableData()
        {
            var builder = new BlobBuilder(Allocator.Persistent);
            ref var placeable = ref builder.ConstructRoot<Bounce.TaleSpire.AssetManagement.PlaceableData>();

            placeable.OrientationOffset = OrientationOffset;
            placeable.Id = Id;
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

            builder.ConstructStringArray(ref placeable.Tags, Tags.ToArray());
            var blobBuilderArray = builder.Allocate(ref placeable.Assets, Assets.Count);

            for (int i = 0; i < Assets.Count; i++)
            {
                var assetBuilder = new BlobBuilder(Allocator.Persistent);
                ref var newPack = ref assetBuilder.ConstructRoot<Bounce.TaleSpire.AssetManagement.AssetLoaderData.Packed>();
                Assets[i].Pack(assetBuilder,Assets[i].assetPackId ?? assetPackId,ref newPack);
                blobBuilderArray[i] = newPack;
            }

            builder.Allocate<AssetScriptIndex>(ref placeable.AssetScripts, 0);
            ConstructEmptyScript(builder, ref placeable, false);
            placeable.IconAtlasIndex = IconAtlasIndex;
            placeable.IconAtlasRegion = IconAtlasRegion;
            placeable.Kind = Kind;
            placeable.GroupTag.Value = GroupTag;

            return placeable;
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
