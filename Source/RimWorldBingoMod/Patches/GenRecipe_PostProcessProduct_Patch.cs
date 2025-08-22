using HarmonyLib;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
	public class GenRecipe_PostProcessProduct_Patch
	{
		public static void Postfix(Thing product)
		{
			ThingProducedBroadcaster.Notify(product);
		}
	}
}