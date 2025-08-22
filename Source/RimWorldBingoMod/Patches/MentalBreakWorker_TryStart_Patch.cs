using HarmonyLib;
using RimWorldBingoMod.Broadcasters;
using Verse;
using Verse.AI;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(MentalBreakWorker), nameof(MentalBreakWorker.TryStart))]
	public class MentalBreakWorker_TryStart_Patch
	{
		public static void Postfix(MentalBreakWorker __instance, bool __result, Pawn pawn)
		{
			if (__result)
			{
				MentalBreakOccuredBroadcaster.Notify(__instance.def, pawn);
			}
		}
	}
}