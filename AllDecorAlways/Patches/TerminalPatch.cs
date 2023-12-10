using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace AllDecorAlways.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    public class TerminalPatch
    {
        [HarmonyPrefix, HarmonyPatch("RotateShipDecorSelection")]
        public static bool AddAllShipDecor(Terminal __instance) {
            IEnumerable<TerminalNode> availableDecor = StartOfRound.Instance.unlockablesList.unlockables
                .Where(u => u.shopSelectionNode != null && !u.alwaysInStock && !u.hasBeenUnlockedByPlayer && !u.alreadyUnlocked)
                .Select(u => u.shopSelectionNode);

            foreach (var unlockable in availableDecor) {
                AllDecorAlwaysBase.Log.LogInfo($"Thing is {unlockable.name}");
            }

            __instance.ShipDecorSelection = availableDecor.ToList();

            AllDecorAlwaysBase.Log.LogInfo("Decor for everyone!");

            return false; // No point running vanilla logic as we want to completely replace it
        }

        [HarmonyPostfix, HarmonyPatch("Awake")]
        public static void ChangeDecorHeading(Terminal __instance) {
            var storeNode = __instance.terminalNodes.allKeywords.First(kw => kw.name == "Store").specialKeywordResult;
            var displayTextLines = storeNode.displayText.Split('\n').ToList();
            var decorStartIndex = displayTextLines.IndexOf("[unlockablesSelectionList]");

            if (decorStartIndex < 0) {
                AllDecorAlwaysBase.Log.LogError("Failed to find decor start index in store node display text!");
                return;
            }

            displayTextLines[decorStartIndex - 2] = "The Company appreciates your hard work, all ship decor is available:";
            storeNode.displayText = string.Join("\n", displayTextLines);
        }
    }
}
