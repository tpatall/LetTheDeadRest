using BepInEx;
using HarmonyLib;
using LethalConfig.ConfigItems.Options;
using LethalConfig.ConfigItems;
using LethalConfig;
using LetTheDeadRest.Patches;
using BepInEx.Configuration;

namespace LetTheDeadRest
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("ainavt.lc.lethalconfig")]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        public static Plugin instance;

        public ConfigEntry<float> volumePercentage;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            InitializeConfigValues();

            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(StartOfRoundPatch));

            this.Logger.LogInfo(PluginInfo.PLUGIN_NAME + " loaded");
        }

        private void InitializeConfigValues()
        {
            LethalConfigManager.SetModDescription(PluginInfo.PLUGIN_NAME);

            volumePercentage = ((BaseUnityPlugin)instance).Config.Bind(
                "LetTheDeadRest",
                "Volume Percentage",
                0.2f,
                "How loud alive players are compared to your fellow dead players. From 0.0 to 1.0."
            );
            FloatSliderOptions volumePercentageSlider = new FloatSliderOptions
            {
                RequiresRestart = false,
                Min = 0f,
                Max = 1f
            };
            FloatSliderConfigItem configItem = new FloatSliderConfigItem(volumePercentage, volumePercentageSlider);
            LethalConfigManager.AddConfigItem(configItem);

            volumePercentage.SettingChanged += delegate
            {
                StartOfRound.Instance.UpdatePlayerVoiceEffects();
            };
        }

        public static void Log(string message)
        {
            instance.Logger.LogInfo($"{message}");
        }

        public static void LogError(string message)
        {
            instance.Logger.LogError(message);
        }
    }
}
