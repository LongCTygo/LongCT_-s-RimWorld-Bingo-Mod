using RimWorld;
using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class MemoryGainedBroadcaster
	{
		public static event Action<Pawn, Pawn, Thought_Memory> Gained;

		public static void Notify(Pawn pawn, Pawn otherPawn, Thought_Memory memory)
		{
			Gained?.Invoke(pawn, otherPawn, memory);
		}
	}
}