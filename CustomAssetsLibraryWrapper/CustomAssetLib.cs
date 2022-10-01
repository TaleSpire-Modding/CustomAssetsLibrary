using System;
using BepInEx;
using Bounce.Unmanaged;
using CustomAssetsCompiler.CoreDTO;

namespace CustomAssetsLibrary
{
    [BepInPlugin(Guid, Name, Version)]
    public sealed class CustomAssetLib : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.PM.plugins.CustomAssetLib";
        public const string Version = "1.1.3.0";
        private const string Name = "Custom Asset Library";

        /// <summary>
        /// Obsolete method, generates binary index file
        /// </summary>
        /// <param name="directory">Where the folder that contains index.json is</param>
        [Obsolete("Please use Generate(string directory, CustomAssetsPlugin.Data.Index index) instead")]
        public static void Generate(string directory)
        {
            CustomAssetsCompiler.TaleWeaverCompiler.Generate(directory);
        }

        /// <summary>
        /// Public interface method to generate a binary INDEX for CAP
        /// Faster as it's already loaded
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="index">Pass this to skip </param>
        public static void Generate(string directory, CustomAssetsPlugin.Data.Index index)
        {
            CustomAssetsCompiler.TaleWeaverCompiler.Generate(directory, index);
        }

        /// <summary>
        /// Create an NGuid from a string via MD5 Hash
        /// </summary>
        /// <param name="id">text to get hashed</param>
        /// <returns>NGuid of the string</returns>
        public static NGuid GenerateID(string id) => CustomAssetsCompiler.TaleWeaverCompiler.GenerateID(id);
    }
}