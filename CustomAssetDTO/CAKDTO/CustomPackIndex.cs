using Unity.Entities;

namespace Bounce.TaleSpire.AssetManagement
{
    public struct CustomPackIndex
    {
        public BlobString Name;
        public BlobArray<CustomKinds> Kinds;
        public BlobArray<Atlas> Atlases;
    }
}
