using NUnit.Framework;

namespace CustomAssetsLibrary.Tests
{
    internal class MusicTest : ITest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void NewTestScriptSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        public void RunTests()
        {
            NewTestScriptSimplePasses();
        }
    }
}