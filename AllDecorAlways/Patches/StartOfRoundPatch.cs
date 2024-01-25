using System.Reflection;
using HarmonyLib;

namespace AllDecorAlways.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	public class StartOfRoundPatch
	{
		[HarmonyPrefix]
		[HarmonyPatch("LoadUnlockables")]
		public static void UnlockAllDecor(UnlockablesList ___unlockablesList) {
			if (!AllDecorAlwaysBase.UnlockAllDecor.Value) {
				return;
			}

			for (int i = 0; i < ___unlockablesList.unlockables.Count; i++) {
				UnlockableItem unlockable = ___unlockablesList.unlockables[i];
				if (unlockable.hasBeenUnlockedByPlayer || unlockable.alreadyUnlocked || unlockable.shopSelectionNode == null || unlockable.alwaysInStock) {
					continue;
				}

				AllDecorAlwaysBase.Log.LogInfo($"Unlocking {unlockable.unlockableName}!");
				unlockable.hasBeenUnlockedByPlayer = true;
				if (unlockable.unlockableType == 0) {
					// Suits
					MethodInfo spawnUnlockableMethodInfo = typeof(StartOfRound).GetMethod("SpawnUnlockable", BindingFlags.NonPublic | BindingFlags.Instance);
					spawnUnlockableMethodInfo.Invoke(StartOfRound.Instance, new object[] { i });
				} else {
					unlockable.inStorage = true;
				}
			}
		}
	}
}
