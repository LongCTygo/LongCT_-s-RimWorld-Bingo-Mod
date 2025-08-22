using RimWorld;
using RimWorldBingoMod.Broadcasters.Biotech;
using Verse;

namespace RimWorldBingoMod.Objectives.Biotech
{
	internal class ImplantXenogermOfComplexityProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			XenogermImplantedBroadcaster.Implanted += OnImplanted;
		}

		protected override void UnregisterFromBroadcaster()
		{
			XenogermImplantedBroadcaster.Implanted -= OnImplanted;
		}

		private void OnImplanted(Pawn pawn, Xenogerm xenogerm)
		{
			AchieveSet(xenogerm.GeneSet.ComplexityTotal);
		}
	}
}