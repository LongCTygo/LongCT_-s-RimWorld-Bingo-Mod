using RimWorldBingoMod.Broadcasters.Odyssey;
using Verse;

namespace RimWorldBingoMod.Objectives.Odyssey
{
	public class FishingProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			FishFishedBroadcaster.Fished += OnFished;
		}

		protected override void UnregisterFromBroadcaster()
		{
			FishFishedBroadcaster.Fished -= OnFished;
		}

		private void OnFished(int amount)
		{
			Achieve(amount);
		}
	}
}