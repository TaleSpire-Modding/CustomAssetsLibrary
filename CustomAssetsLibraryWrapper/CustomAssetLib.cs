using BepInEx;
using Bounce.Unmanaged;
using LordAshes;

namespace CustomAssetsLibrary
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(SmartConvert.Guid)]
    public class CustomAssetLib : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.PM.plugins.CustomAssetLib";
        public const string Version = "1.1.1.1";
        private const string Name = "Custom Asset Library";

        // Public interface method to generate a binary INDEX for CAP
        public static void Generate(string directory)
            => CustomAssetsCompiler.TaleWeaverCompiler.Generate(directory);

        /// <summary>
        /// Create an NGuid from a string via MD5 Hash
        /// </summary>
        /// <param name="id">text to get hashed</param>
        /// <returns>NGuid of the string</returns>
        public static NGuid GenerateID(string id) => CustomAssetsCompiler.TaleWeaverCompiler.GenerateID(id);
    }
}