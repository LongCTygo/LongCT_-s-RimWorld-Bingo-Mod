using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(ResearchManager), nameof(ResearchManager.FinishProject))]
	public class ResearchManager_FinishProject_Patch
	{
		public static void Postfix(ResearchProjectDef proj)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				ResearchCompletionBroadcaster.Notify(proj);
			}
		}
	}
}