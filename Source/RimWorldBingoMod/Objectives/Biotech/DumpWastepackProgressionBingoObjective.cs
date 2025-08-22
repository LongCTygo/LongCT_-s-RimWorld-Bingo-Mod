using RimWorldBingoMod.Broadcasters.Biotech;
using Verse;

namespace RimWorldBingoMod.Objectives.Biotech
{
	public class DumpWastepackProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			WastepackDumpedBroadcaster.Dumped += OnDumped;
		}

		protected override void UnregisterFromBroadcaster()
		{
			WastepackDumpedBroadcaster.Dumped -= OnDumped;
		}

		private void OnDumped(int amount)
		{
			Achieve(amount);
		}
	}
}