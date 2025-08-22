using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(CompEggLayer), nameof(CompEggLayer.ProduceEgg))]
	public class CompEggLayer_ProduceEgg_Patch
	{
		public static void Postfix(Thing __result)
		{
			AnimalProductProducedBroadcaster.Notify(__result);
		}
	}
}