using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace AllDecorAlways
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class AllDecorAlwaysBase : BaseUnityPlugin
    {
        public const string ModGUID = "stormytuna.AllDecorAlways";
        public const string ModName = "AllDecorAlways";
        public const string ModVersion = "1.1.0";

        public static ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
        public static AllDecorAlwaysBase Instance;

        private readonly Harmony harmony = new Harmony(ModGUID);

        private void Awake() {
            if (Instance is null) {
                Instance = this;
            }

            Log.LogInfo("All Decor Always has awoken!");

            LoadConfigs();

            harmony.PatchAll();
        }

        #region Configs

        public static ConfigEntry<bool> UnlockAllDecor;

        private void LoadConfigs() {
            UnlockAllDecor = Config.Bind("Cheats", "UnlockAllDecor", false, "Whether or not all decor should be unlocked immediately rather than having to be bought");
        }

        #endregion
    }
}
