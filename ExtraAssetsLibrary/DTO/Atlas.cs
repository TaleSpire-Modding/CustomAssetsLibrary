using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CustomAssetsLibrary.DTO
{
    public class Atlas
    {
        public string LocalPath = "";
        public int SizeX;
        public int SizeY;

        internal Bounce.TaleSpire.AssetManagement.Atlas ToBRAtlas()
        {
            var builder = new BlobBuilder(Allocator.Persistent);
            ref var output = ref builder.ConstructRoot<Bounce.TaleSpire.AssetManagement.Atlas>();
            builder.AllocateString(ref output.LocalPath,LocalPath);
            output.Size = new int2(SizeX, SizeY);

            return output;
        }
    }
}
