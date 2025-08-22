using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters.Royalty;

namespace RimWorldBingoMod.Patches.Royalty
{
	[HarmonyPatch(typeof(Pawn_RoyaltyTracker), nameof(Pawn_RoyaltyTracker.TryUpdateTitle))]
	public class PawnRoyaltyTracker_SetTitle_Patch
	{
		public static void Postfix(Pawn_RoyaltyTracker __instance, bool __result, RoyalTitleDef updateTo)
		{
			if (__result)
			{
				if (__instance.pawn.Faction == Faction.OfPlayer)
				{
					RoyalTitleGainedBroadcaster.Notify(updateTo);
				}
			}
		}
	}
}