using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(StoryWatcher_PopAdaptation), nameof(StoryWatcher_PopAdaptation.Notify_PawnEvent))]
	public class StoryWatcher_PopAdaption_NotifyPawnEvent_Patch
	{
		public static void Postfix(Pawn p, PopAdaptationEvent ev)
		{
			if (p.RaceProps.Humanlike && ev == PopAdaptationEvent.GainedColonist)
			{
				ColonistRecruitedBroadcaster.Notify(p);
			}
		}
	}
}