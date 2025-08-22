using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Objectives.Thoughts
{
	public class LovinProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			MemoryGainedBroadcaster.Gained += OnMemoryGained;
		}

		protected override void UnregisterFromBroadcaster()
		{
			MemoryGainedBroadcaster.Gained -= OnMemoryGained;
		}

		private void OnMemoryGained(Pawn pawn, Pawn otherPawn, Thought_Memory memory)
		{
			if (!pawn.IsColonist)
			{
				return;
			}
			if (memory != null && memory.def == ThoughtDefOf.GotSomeLovin)
			{
				if (otherPawn == null)
				{
					return;
				}
				if (otherPawn.IsColonist)
				{
					Achieve(0.5f); //The other will add 0.5
				}
				else
				{
					Achieve(); //Cross-faction lovin
				}
			}
		}
	}
}