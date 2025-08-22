using RimWorldBingoMod.Broadcasters.Royalty;
using Verse;

namespace RimWorldBingoMod.Objectives.Royalty
{
	public class TechprintAppliedProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			TechprintAppliedBroadcaster.Applied += OnApplied;
		}

		protected override void UnregisterFromBroadcaster()
		{
			TechprintAppliedBroadcaster.Applied -= OnApplied;
		}

		private void OnApplied(ResearchProjectDef def)
		{
			Achieve();
		}
	}
}