using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorldBingoMod.Utils
{
	public static class BingoRewardsUtils
	{
		public static void SendRewards(IEnumerable<Thing> things, Map map, bool isLastItem = false, bool isPrestige = false)
		{
			if (!things.Any())
			{
				return;
			}
			var spot = DropCellFinder.RandomDropSpot(map);
			DropPodUtility.DropThingsNear(spot, map, things, canRoofPunch: false);
			//Send letter
			string label = "Bingo.Letter.ArchotechRewardArrived.label".Translate();
			string text;
			if (isLastItem)
			{
				text = "Bingo.Letter.ArchotechFinalRewardRewardArrived.text".Translate();
			}
			else if (isPrestige)
			{
				text = "Bingo.Letter.ArchotechRewardArrivedPrestige.text".Translate();
			}
			else
			{
				text = "Bingo.Letter.ArchotechRewardArrived.text".Translate();
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, new LookTargets(things));
		}
	}
}