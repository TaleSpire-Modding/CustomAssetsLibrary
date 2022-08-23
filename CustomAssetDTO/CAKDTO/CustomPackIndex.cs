using Unity.Entities;

namespace Bounce.TaleSpire.AssetManagement
{
    public struct CustomPackIndex
    {
        public BlobString Name;
        public BlobArray<CustomCategories> Catagories;
        public BlobArray<Atlas> Atlases;
    }
}
