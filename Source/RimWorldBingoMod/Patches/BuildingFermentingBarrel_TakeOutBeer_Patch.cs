using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(Building_FermentingBarrel), nameof(Building_FermentingBarrel.TakeOutBeer))]
	public class BuildingFermentingBarrel_TakeOutBeer_Patch
	{
		public static void Postfix(Thing __result)
		{
			if (__result == null) return;
			FermentingBarrelTakenOutBroadcaster.Notify(__result);
		}
	}
}