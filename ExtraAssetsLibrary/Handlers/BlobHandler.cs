using System;
using Bounce.BlobAssets;
using Bounce.TaleSpire.AssetManagement;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CustomAssetsLibrary.Handlers
{
    public static class BlobHandler
    {
        public static BlobArray<BlobString> ConstructBlobData(string[] tags)
        {
            if (tags == null) tags = new string[0];
            var builder = new BlobBuilder(Allocator.Temp);
            try
            {
                ref var root = ref builder.ConstructRoot<BlobArray<BlobString>>();
                var nodearray = builder.Allocate(ref root, tags.Length);
                for (var i = 0; i < tags.Length; i++) builder.AllocateString(ref nodearray[i], tags[i]);
                return builder.CreateBlobAssetReference<BlobArray<BlobString>>(Allocator.Persistent).Value;
            }
            catch (Exception e)
            {
                if (CustomAssetPlugin.LogLevel.Value >= LogLevel.Low) Debug.Log($"Extra Asset Library Plugin:ConstructBlobError:{e}");
            }

            return builder.CreateBlobAssetReference<BlobArray<BlobString>>(Allocator.Persistent).Value;
        }

        internal static BlobView<CreatureData> ToView(CreatureData cdata)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var root = ref builder.ConstructRoot<BlobArray<CreatureData>>();
            var nodeArray = builder.Allocate(ref root, 1);
            nodeArray[0] = cdata;
            var blobArray = builder.CreateBlobAssetReference<BlobArray<CreatureData>>(Allocator.Persistent).Value;
            var response = blobArray.TakeView(0);
            response.Value.Name = cdata.Name;
            response.Value.DefaultScale = cdata.DefaultScale;
            response.Value.BaseRadius = cdata.BaseRadius;
            return response;
        }
    }
}