using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Bounce.Unmanaged;
using UnityEngine;
using CustomAssetsLoader.ReflecExt;
using ModdingTales;

namespace CustomAssetsLoader.Patches
{
    [HarmonyPatch(typeof(AssetLibraryDbCategory), "SetupData")]
    internal sealed class AssetLibraryDbCategorySetupDataPatch
    {
        internal static Dictionary<NGuid, string> registerPacks = new Dictionary<NGuid, string>();
        /// <summary>
        /// Public for Testing only
        /// </summary>
        public static void Prefix(ref List<AssetCampaignSetting>  ___includedSettings)
        {
            int i = 0;

            foreach (var pack in registerPacks)
            {

                CustomAssetLoader ._logger.LogInfo(pack);
                if (___includedSettings.All(c => c.CampaignSettingName != pack.Value))
                {
                    var instance = ScriptableObject.CreateInstance<AssetCampaignSetting>();
                    instance.SetValue("campaignSettingName",pack.Value);

                    var includedPackageIdStrings = instance.GetValue<AssetCampaignSetting,List<string>>("includedPackageIdStrings");
                    includedPackageIdStrings.Add(pack.Key.ToString());

                    instance.IncludedPackageIds.Add(pack.Key);
                    instance.IncludeInSearch = true;
                    instance.Expanded = true;

                    ___includedSettings.Add(instance);

                    if (CustomAssetLoader.LogLevel > ModdingUtils.LogLevel.Medium)
                        CustomAssetLoader._logger.LogInfo($"Added new settings with {pack.Key} {pack.Value}");
                }
                else if (!___includedSettings.Single(c => c.CampaignSettingName == pack.Value).IncludedPackageIds
                        .Contains(pack.Key))
                {
                    var instance = ___includedSettings.Single(c => c.CampaignSettingName == pack.Value);
                    var includedPackageIdStrings = instance.GetValue<AssetCampaignSetting, List<string>>("includedPackageIdStrings");
                    includedPackageIdStrings.Add(pack.Key.ToString());
                    instance.IncludedPackageIds.Add(pack.Key);

                    if (CustomAssetLoader.LogLevel > ModdingUtils.LogLevel.Medium)
                        CustomAssetLoader._logger.LogInfo($"Added {pack.Key} {pack.Value} to existing settings");
                }
            }
        }
    }

    [HarmonyPatch(typeof(AssetLibraryDbCategory), "GetFolderGroupCount")]
    internal sealed class AssetLibraryDbGetFolderGroupCountPatch
    {
        public static bool Prefix(ref List<AssetCampaignSetting> ___includedSettings, ref int __result)
        {
            __result = ___includedSettings.Count;
            return false;
        }
    }

    [HarmonyPatch(typeof(AssetLibraryDbCategory), "GetFolderGroupName")]
    internal sealed class AssetLibraryDbGetFolderGroupNamePatch
    {
        public static bool Prefix(ref List<AssetCampaignSetting> ___includedSettings, ref int index, ref string __result)
        {
            __result = ___includedSettings[index].CampaignSettingName;
            return false;
        }
    }
}
