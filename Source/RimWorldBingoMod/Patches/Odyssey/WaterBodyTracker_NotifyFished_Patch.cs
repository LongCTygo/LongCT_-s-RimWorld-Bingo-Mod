using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters.Odyssey;

namespace RimWorldBingoMod.Patches.Odyssey
{
	[HarmonyPatch(typeof(WaterBodyTracker), nameof(WaterBodyTracker.Notify_Fished))]
	public class WaterBodyTracker_NotifyFished_Patch
	{
		public static void Postfix(float amount)
		{
			FishFishedBroadcaster.Notify((int)amount);
		}
	}
}