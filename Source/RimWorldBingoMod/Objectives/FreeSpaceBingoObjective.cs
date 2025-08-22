using Verse;

namespace RimWorldBingoMod.Objectives
{
	public class FreeSpaceBingoObjective : BingoObjective
	{
		public override TaggedString Title => def.title.Translate();

		public override TaggedString Details => def.details.Translate();

		public override bool IsConditionAlreadyAchieved()
		{
			return true;
		}

		protected override void RegisterToBroadcaster()
		{
		}

		protected override void UnregisterFromBroadcaster()
		{
		}
	}
}