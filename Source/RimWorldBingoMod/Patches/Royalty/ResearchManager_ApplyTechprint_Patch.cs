using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters.Royalty;
using Verse;

namespace RimWorldBingoMod.Patches.Royalty
{
	[HarmonyPatch(typeof(ResearchManager), nameof(ResearchManager.ApplyTechprint))]
	public class ResearchManager_ApplyTechprint_Patch
	{
		public static void Postfix(ResearchProjectDef proj)
		{
			TechprintAppliedBroadcaster.Notify(proj);
		}
	}
}