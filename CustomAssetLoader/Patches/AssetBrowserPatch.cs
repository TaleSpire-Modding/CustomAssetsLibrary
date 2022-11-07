using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bounce.Unmanaged;

namespace CustomAssetsLoader.Patches
{
    [HarmonyPatch(typeof(AssetLibraryDbCategory), "SetupData")]
    internal sealed class AssetBrowserPatch
    {
        private static Dictionary<NGuid, string> registerPacks = new Dictionary<NGuid, string>();
        /// <summary>
        /// Public for Testing only
        /// </summary>
        public static void Prefix(ref List<AssetCampaignSetting>  ___includedSettings)
        {
            foreach (var pack in registerPacks)
            {
                AssetCampaignSetting newpack;

                if (___includedSettings.All(c => c.CampaignSettingName != pack.Value))
                {
                    newpack = new AssetCampaignSetting
                    {
                        name = pack.Value
                    };
                    ___includedSettings.Add(newpack);

                }
                newpack = ___includedSettings.Single(c => c.CampaignSettingName == pack.Value);

            }
        }
    }
}
