using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Objectives.Mood
{
	public class InspirationProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			InspirationGainedBroadcaster.Gained += OnGained;
		}

		protected override void UnregisterFromBroadcaster()
		{
			InspirationGainedBroadcaster.Gained -= OnGained;
		}

		private void OnGained(InspirationDef def)
		{
			Achieve();
		}
	}
}