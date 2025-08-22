using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Objectives.Quest
{
	public class QuestShipBingoObjective : BingoObjective
	{
		public override TaggedString Title => def.title.Translate();

		public override TaggedString Details => def.details.Translate();

		protected override void RegisterToBroadcaster()
		{
			CaravanArrivedAtShipBroadcaster.Arrived += OnArrived;
		}

		protected override void UnregisterFromBroadcaster()
		{
			CaravanArrivedAtShipBroadcaster.Arrived -= OnArrived;
		}

		public void OnArrived()
		{
			Achieve();
		}
	}
}