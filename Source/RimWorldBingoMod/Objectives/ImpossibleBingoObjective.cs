using Verse;

namespace RimWorldBingoMod.Objectives
{
	public class ImpossibleBingoObjective : BingoObjective
	{
		public override TaggedString Title => def.title.Translate();

		public override TaggedString Details => def.details.Translate();

		protected override void RegisterToBroadcaster()
		{
		}

		protected override void UnregisterFromBroadcaster()
		{
		}
	}
}