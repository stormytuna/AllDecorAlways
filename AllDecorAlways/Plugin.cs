using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AllDecorAlways
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class AllDecorAlwaysBase : BaseUnityPlugin
    {
        public const string ModGUID = "stormytuna.AllDecorAlways";
        public const string ModName = "AllDecorAlways";
        public const string ModVersion = "1.0.0";

        public static ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
        public static AllDecorAlwaysBase Instance;

        private readonly Harmony harmony = new Harmony(ModGUID);

        private void Awake() {
            if (Instance is null) {
                Instance = this;
            }

            Log.LogInfo("All Decor Always has awoken!");

            harmony.PatchAll();
        }
    }
}
