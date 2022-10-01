using BepInEx;
using BepInEx.Configuration;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PluginMasters
{
    public partial class CustomAssetsLibraryPlugin : BaseUnityPlugin
    {
        public static class Utility
        {
            /// <summary>
            /// Function to check if the board is loaded
            /// </summary>
            /// <returns></returns>
            public static bool isBoardLoaded()
            {
                return CameraController.HasInstance && BoardSessionManager.HasInstance && !BoardSessionManager.IsLoading;
            }

            /// <summary>
            /// Method to properly evaluate shortcut keys. 
            /// </summary>
            /// <param name="check"></param>
            /// <returns></returns>
            public static bool StrictKeyCheck(KeyboardShortcut check)
            {
                if (!check.IsUp()) { return false; }
                foreach (KeyCode modifier in new KeyCode[] { KeyCode.LeftAlt, KeyCode.RightAlt, KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftShift, KeyCode.RightShift })
                {
                    if (Input.GetKey(modifier) != check.Modifiers.Contains(modifier)) { return false; }
                }
                return true;
            }

            /// <summary>
            /// Method to generate a Guid from a string
            /// </summary>
            /// <param name="input">Text</param>
            /// <returns>Guid based on the input text</returns>
            public static Guid GuidFromString(string input)
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
                    return new Guid(hash);
                }
            }

            /// <summary>
            /// Method to obtain the Base Loader Game Object based on a CreatureGuid
            /// </summary>
            /// <param name="cid">Creature Guid</param>
            /// <returns>BaseLoader Game Object</returns>
            public static GameObject GetBaseLoader(CreatureGuid cid)
            {
                try
                {
                    CreatureBoardAsset asset = null;
                    CreaturePresenter.TryGetAsset(cid, out asset);
                    if (asset != null)
                    {
                        Transform _base = null;
                        StartWith(asset, "_base", ref _base);
                        Transform baseLoader = null;
                        Traverse(_base, "BaseLoader", ref baseLoader);
                        if (baseLoader != null)
                        {
                            Debug.Log("Custom Assets Library Plugin Integrated Extension:  Base Loader '" + baseLoader.GetChild(0).name + "' Found");
                            return baseLoader.GetChild(0).gameObject;
                        }
                        else
                        {
                            Debug.LogWarning("Custom Assets Library Plugin Integrated Extension: Could Not Find Base Loader");
                            return null;
                        }
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// Method to obtain the Asset Loader Game Object based on a CreatureGuid
            /// </summary>
            /// <param name="cid">Creature Guid</param>
            /// <returns>AssetLoader Game Object</returns>
            public static GameObject GetAssetLoader(CreatureGuid cid)
            {
                try
                {
                    CreatureBoardAsset asset = null;
                    CreaturePresenter.TryGetAsset(cid, out asset);
                    if (asset != null)
                    {
                        Transform _creatureRoot = null;
                        StartWith(asset, "_creatureRoot", ref _creatureRoot);
                        Transform assetLoader = null;
                        Traverse(_creatureRoot, "AssetLoader", ref assetLoader);
                        if (assetLoader != null)
                        {
                            Debug.Log("Custom Assets Library Plugin Integrated Extension:  Asset Loader '" + assetLoader.GetChild(0).name + "' Found");
                            return assetLoader.GetChild(0).gameObject;
                        }
                        else
                        {
                            Debug.LogWarning("Custom Assets Library Plugin Integrated Extension: Could Not Find Asset Loader");
                            return null;
                        }
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }

            public static void StartWith(CreatureBoardAsset asset, string seek, ref Transform match)
            {
                try
                {
                    Type type = typeof(CreatureBoardAsset);
                    match = null;
                    foreach (FieldInfo fi in type.GetRuntimeFields())
                    {
                        if (fi.Name == seek) { match = (Transform)fi.GetValue(asset); break; }
                    }
                }
                catch
                {
                    match = null;
                }
            }

            public static void Traverse(Transform root, string seek, ref Transform match)
            {
                try
                {
                    // Debug.Log("Seeking Child Named '" + seek + "'. Found '" + root.name + "'");
                    if (match != null) { return; }
                    if (root.name == seek) { match = root; return; }
                    foreach (Transform child in root.Children())
                    {
                        Traverse(child, seek, ref match);
                    }
                }
                catch
                {
                }
            }

            public static float ParseFloat(string value)
            {
                return float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string GetCreatureName(string nameBlock)
            {
                if (nameBlock==null) { return "(Unknown)"; }
                if (!nameBlock.Contains("<size=0>")) { return nameBlock; }
                return nameBlock.Substring(0, nameBlock.IndexOf("<size=0>")).Trim();
            }

            public static void PostOnMainPage(System.Reflection.MemberInfo plugin)
            {
                SceneManager.sceneLoaded += (scene, mode) =>
                {
                    try
                    {
                        if (scene.name == "UI")
                        {
                            TextMeshProUGUI betaText = GetUITextByName("BETA");
                            if (betaText)
                            {
                                betaText.text = "INJECTED BUILD - unstable mods";
                            }
                        }
                        else
                        {
                            TextMeshProUGUI modListText = GetUITextByName("TextMeshPro Text");
                            if (modListText)
                            {
                                BepInPlugin bepInPlugin = (BepInPlugin)Attribute.GetCustomAttribute(plugin, typeof(BepInPlugin));
                                if (modListText.text.EndsWith("</size>"))
                                {
                                    modListText.text += "\n\nMods Currently Installed:\n";
                                }
                                modListText.text += "\nPlugin Masters' " + bepInPlugin.Name + " - " + bepInPlugin.Version;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex);
                    }
                };
            }

            private static TextMeshProUGUI GetUITextByName(string name)
            {
                TextMeshProUGUI[] texts = UnityEngine.Object.FindObjectsOfType<TextMeshProUGUI>();
                for (int i = 0; i < texts.Length; i++)
                {
                    if (texts[i].name == name)
                    {
                        return texts[i];
                    }
                }
                return null;
            }
        }
    }
}
