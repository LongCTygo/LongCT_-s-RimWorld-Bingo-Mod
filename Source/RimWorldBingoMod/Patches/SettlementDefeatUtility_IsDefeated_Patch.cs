using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using RimWorldBingoMod.Broadcasters;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(SettlementDefeatUtility), nameof(SettlementDefeatUtility.IsDefeated))]
	public class SettlementDefeatUtility_IsDefeated_Patch
	{
		public static void Postfix(bool __result, Faction faction)
		{
			if (__result)
			{
				FactionSettlementDestroyedBroadcaster.Notify(faction);
			}
		}
	}
}