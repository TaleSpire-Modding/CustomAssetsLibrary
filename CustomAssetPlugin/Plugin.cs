using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using UnityEngine;


namespace PluginMasters
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(LordAshes.FileAccessPlugin.Guid, BepInDependency.DependencyFlags.HardDependency)]
    public partial class CustomAssetsLibraryPlugin : BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "Custom Assets Library Plugin";
        public const string Guid = "org.lordashes.plugins.customassetslibrary";
        public const string Version = "3.1.0.0";

        // Public Enum
        public enum DiagnosticMode
        {
            none = 0,
            low = 1,
            high = 2,
            ultra = 3
        }

        public enum OperationMode
        {
            rebuildIndexAlways = 0,
            rebuildIndexIfMissing = 1,
            rebuildNever = 2
        }

        // Configuration
        private ConfigEntry<OperationMode> operationMode { get; set; }
        private ConfigEntry<DiagnosticMode> diagnosticMode { get; set; }
        private ConfigEntry<bool> createJSONIndexFiles { get; set; }

        public static CustomAssetsLibraryPlugin _self = null;

        void Awake()
        {
            _self = this;
            
            operationMode = Config.Bind("Settings", "Build Index Mode", CustomAssetsLibraryPlugin.OperationMode.rebuildIndexIfMissing);
            diagnosticMode = Config.Bind("Troubleshooting", "Diagnostic Mode", DiagnosticMode.high);
            createJSONIndexFiles = Config.Bind("Troubleshooting", "Create Reference JSON Index Files", false);

            UnityEngine.Debug.Log("Custom Asset Library Plugin: " + this.GetType().AssemblyQualifiedName + " Active. (Diagnostic Mode = "+Diagnostics()+")");

            if (operationMode.Value != OperationMode.rebuildNever)
            {
                Setup.RegisterAssets();
            }

            Utility.PostOnMainPage(this.GetType());
        }

        void Update()
        {
            if (Utility.isBoardLoaded())
            {
                // Board is loaded

                if (Setup.violations.Count > 0)
                {
                    SystemMessage.DisplayInfoText("Custom Asset Library Plugin:\r\nPrime Directive Violation(s)!\r\nSee Log For Violations List", 10f);
                    Debug.LogWarning("Custom Asset Plugin: List Of Prime Directive Violations:");
                    foreach (string violation in Setup.violations)
                    {
                        Debug.LogWarning("Custom Asset Plugin: -> " + violation);
                    }
                    Setup.violations.Clear();
                }
            }
        }

        public static DiagnosticMode Diagnostics()
        {
            return CustomAssetsLibraryPlugin._self.diagnosticMode.Value;
        }

        public static OperationMode OperationsMode()
        {
            return CustomAssetsLibraryPlugin._self.operationMode.Value;
        }
        public static bool CreateJsonReferenceFiles()
        {
            return CustomAssetsLibraryPlugin._self.createJSONIndexFiles.Value;
        }
    }
}
