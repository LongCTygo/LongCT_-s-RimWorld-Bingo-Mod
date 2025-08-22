using HarmonyLib;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(Pawn), nameof(Pawn.Kill))]
	public class Pawn_Kill_Patch
	{
		public static void Postfix(Pawn __instance, DamageInfo? dinfo)
		{
			PawnKilledBroadcaster.Notify(__instance, dinfo);
		}
	}
}