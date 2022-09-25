using Unity.Entities;
using Unity.Mathematics;

namespace CustomAssetsCompiler.CoreDTO
{
    public sealed class Atlas
    {
        public string LocalPath = "";
        public int SizeX = 128;
        public int SizeY = 128;

        internal Bounce.TaleSpire.AssetManagement.Atlas ToBRAtlas(BlobBuilder builder)
        {
            ref var output = ref builder.ConstructRoot<Bounce.TaleSpire.AssetManagement.Atlas>();
            builder.AllocateString(ref output.LocalPath,LocalPath);
            output.Size = new int2(SizeX, SizeY);
            return output;
        }

        internal void ToBRAtlasData(BlobBuilder builder, ref Bounce.TaleSpire.AssetManagement.Atlas atlas)
        {
            builder.AllocateString(ref atlas.LocalPath, LocalPath);
            atlas.Size.x = SizeX;
            atlas.Size.y = SizeY;
        }
    }
}
