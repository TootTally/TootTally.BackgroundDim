using BaboonAPI.Hooks.Initializer;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using TootTally.Utils;
using TootTally.Utils.TootTallySettings;

namespace TootTally.BackgroundDim
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("TootTally", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin, ITootTallyModule
    {
        public static Plugin Instance;

        private const string CONFIG_NAME = "BackgroundName.cfg";
        public Options option;
        public ConfigEntry<bool> ModuleConfigEnabled { get; set; }
        public bool IsConfigInitialized { get; set; }
        public string Name { get => PluginInfo.PLUGIN_NAME; set => Name = value; }
        public ManualLogSource GetLogger { get => Logger; }
        public static TootTallySettingPage settingPage;
        public void LogInfo(string msg) => Logger.LogInfo(msg);
        public void LogError(string msg) => Logger.LogError(msg);

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;

            GameInitializationEvent.Register(Info, TryInitialize);
        }

        private void TryInitialize()
        {
            ModuleConfigEnabled = TootTally.Plugin.Instance.Config.Bind("Modules", "Background Dim", true, "Adds an adjustable dim effect to the background.");
            TootTally.Plugin.AddModule(this);
        }

        public void LoadModule()
        {
            string configPath = Path.Combine(Paths.BepInExRootPath, "config/");
            ConfigFile config = new ConfigFile(configPath + CONFIG_NAME, true);
            option = new Options()
            {
                DimAmount = config.Bind("General", "Background Dim Amount", 0.75f, "The amount to dim the background by as a percentage."),
                //ShowBGOnBreak = config.Bind("General.Toggles", "No Dim on Break", true, "Reduce dimming when there are no notes to be played.") //WIP
            };

           settingPage = TootTallySettingsManager.AddNewPage("BGDim", "Background Dim", 40, new UnityEngine.Color(.1f, .1f, .1f, .1f));
            if (settingPage != null)
            {
                settingPage.AddSlider("DimSlider", 0, 1, 350, "Dim Amount", option.DimAmount, false);
            }

            Harmony.CreateAndPatchAll(typeof(BackgroundDimController), PluginInfo.PLUGIN_GUID);
            LogInfo($"Module loaded!");
        }

        public void UnloadModule()
        {
            Harmony.UnpatchID(PluginInfo.PLUGIN_GUID);
            settingPage.Remove();
            LogInfo($"Module unloaded!");
        }

        public class Options
        {

            public ConfigEntry<float> DimAmount { get; set; }
            public ConfigEntry<bool> ShowBGOnBreak { get; set; }

        }
    }
}