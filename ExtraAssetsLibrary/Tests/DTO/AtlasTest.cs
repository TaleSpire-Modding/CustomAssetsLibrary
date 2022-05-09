using System.IO;
using Bounce.TaleSpire.AssetManagement;
using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Atlas = CustomAssetsLibrary.DTO.Atlas;

namespace CustomAssetsLibrary.Tests
{
    internal class AtlasTest: ITest
    {
        // A Test behaves as an ordinary method
        public void CanConvert()
        {
            var dtoAtlas = new Atlas
            {
                LocalPath = "hibijibi",
                SizeX = 32,
                SizeY = 64
            };

            var brAtlas = dtoAtlas.ToBRAtlas(new BlobBuilder(Allocator.Persistent));
            Assert.True(brAtlas.LocalPath.ToString() == dtoAtlas.LocalPath);
            Assert.True(brAtlas.Size.x == dtoAtlas.SizeX);
            Assert.True(brAtlas.Size.y == dtoAtlas.SizeY);
        }

        public void RunTests()
        {
            CanConvert();
        }
    }
}