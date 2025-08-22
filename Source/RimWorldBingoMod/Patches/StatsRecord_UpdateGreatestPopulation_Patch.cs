using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(StatsRecord), nameof(StatsRecord.UpdateGreatestPopulation))]
	public class StatsRecord_UpdateGreatestPopulation_Patch
	{
		public static void Postfix(StatsRecord __instance)
		{
			GreatestPopulationBroadcaster.Notify(__instance.greatestPopulation);
		}
	}
}