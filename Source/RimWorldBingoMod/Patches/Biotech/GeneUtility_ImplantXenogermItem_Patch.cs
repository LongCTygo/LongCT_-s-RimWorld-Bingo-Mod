using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters.Biotech;
using Verse;

namespace RimWorldBingoMod.Patches.Biotech
{
	[HarmonyPatch(typeof(GeneUtility), nameof(GeneUtility.ImplantXenogermItem))]
	public class GeneUtility_ImplantXenogermItem_Patch
	{
		public static void Postfix(Pawn pawn, Xenogerm xenogerm)
		{
			XenogermImplantedBroadcaster.Notify(pawn, xenogerm);
		}
	}
}