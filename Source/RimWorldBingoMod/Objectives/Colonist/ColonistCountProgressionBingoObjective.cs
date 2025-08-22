using RimWorld;
using RimWorldBingoMod.Broadcasters;
using System;
using Verse;

namespace RimWorldBingoMod.Objectives.Colonist
{
	public class ColonistCountProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		public override bool IsConditionAlreadyAchieved()
		{
			progress = Math.Max(Find.StoryWatcher.statsRecord.greatestPopulation, progress);
			Log.Message($"Colonist Count ForceCheck: pop is {progress}.");
			return base.IsConditionAlreadyAchieved();
		}

		protected override void RegisterToBroadcaster()
		{
			GreatestPopulationBroadcaster.Updated += OnGreatestPopUpdated;
		}

		protected override void UnregisterFromBroadcaster()
		{
			GreatestPopulationBroadcaster.Updated -= OnGreatestPopUpdated;
		}

		public void OnGreatestPopUpdated(int maxPop)
		{
			AchieveSet(maxPop);
		}
	}
}