using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(InspirationHandler), nameof(InspirationHandler.TryStartInspiration))]
	public class InspirationHander_TryStartInspiration_Patch
	{
		public static void Postfix(InspirationDef def, Pawn ___pawn, bool __result)
		{
			if (!__result)
			{
				return;
			}
			if (___pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			InspirationGainedBroadcaster.Notify(def);
		}
	}
}