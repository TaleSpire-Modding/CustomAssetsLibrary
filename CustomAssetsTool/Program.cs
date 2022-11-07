using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LordAshes
{
    class CustomAssetsTool
    {
        static void Main(string[] args)
        {
            string folder = Environment.CurrentDirectory;
            folder = @"D:\Steam\steamapps\common\TaleSpire\TaleSpire_CustomData";
            Data.Index index = new Data.Index();
            Console.WriteLine("Custom Asset Plugin: Scanning Folder '" + folder + "'.");
            List<string> files = GetSubFiles(folder);
            foreach (string assetFile in files)
            {
                Console.WriteLine("Custom Asset Plugin: Analyzing File '" + assetFile + "'.");
                // Potential Asset Bundle
                AssetBundle ab = null;
                Data.AssetInfo info = null;
                Texture2D portrait = null;
                try
                {
                    // Check Asset Bundle
                    ab = UnityEngine.AssetBundle.LoadFromFile(assetFile);
                }
                catch (Exception x)
                {
                    Console.WriteLine("Custom Asset Plugin: File '" + assetFile + "' Does Not Seem To Be A Valid Asset Bundle.");
                    Console.WriteLine("Custom Asset Plugin: File '" + assetFile + "' Generated " + x);
                    continue;
                }
                try
                {
                    // Check Info File
                    info = JsonConvert.DeserializeObject<Data.AssetInfo>(ab.LoadAsset<TextAsset>("Info.txt").text);
                }
                catch (Exception)
                {
                    Console.WriteLine("Custom Asset Plugin: AssetBundle '" + assetFile + "' Does Not Have A Info.Txt File. Using Default.");
                    info = new Data.AssetInfo()
                    {
                        name = System.IO.Path.GetFileNameWithoutExtension(assetFile),
                        kind = "Creature",
                        category = "Creature",
                        groupName = "Custom Content",
                    };
                }
                try
                {
                    // Check Portrait
                    portrait = ab.LoadAsset<Texture2D>("Portrait.png");
                }
                catch (Exception)
                {
                    Console.WriteLine("Custom Asset Plugin: AssetBundle '" + assetFile + "' Does Not Have A Portrait.Png File. Using Default."); 
                    portrait = LoadTexture("DefaultPortrait.png");
                }
                if (info != null)
                {
                    info.id = GuidFromString(assetFile).ToString();
                    if (!folder.Contains("TaleSpire_CustomData"))
                    {
                        info.location = assetFile.Substring(folder.Length + 1);
                        info.location = info.location.Substring(info.location.IndexOf("/") + 1);
                    }
                    else
                    {
                        info.location = assetFile.Substring(assetFile.IndexOf("TaleSpire_CustomData") + "TaleSpire_CustomData".Length + 1);
                    }
                    if (info.kind == "") { info.kind = "Creature"; }
                    if (info.category == "") { info.category = "Creature"; }
                    Setup.RegisterAsset(info);
                    Setup.CreatePortrait(info, portrait);
                    Setup.CreateLibraryEntry(info);
                }
            }
            if ((index.Creatures.Count + index.Music.Count + index.Props.Count + index.Tiles.Count) > 0)
            {
                Console.WriteLine("Custom Asset Plugin: Writing " + folder + "Index.Json File"); 
                System.IO.File.WriteAllText(folder + "index.json", JsonConvert.SerializeObject(index, Formatting.Indented));
            }
        }

        private static List<string> GetSubFiles(string root)
        {
            List<string> results = new List<string>();
            results.AddRange(System.IO.Directory.EnumerateFiles(root,"*."));
            foreach (string directory in  System.IO.Directory.EnumerateDirectories(root,"*."))
            {
                results.AddRange(GetSubFiles(directory));
            }
            return results;
        }

        public static Guid GuidFromString(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
                return new Guid(hash);
            }
        }

        private static Texture2D LoadTexture(string source)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(System.IO.File.ReadAllBytes(source));
            return tex;
        }
    }
}
