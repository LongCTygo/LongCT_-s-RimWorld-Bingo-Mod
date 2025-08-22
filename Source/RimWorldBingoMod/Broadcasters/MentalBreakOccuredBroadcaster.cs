using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class MentalBreakOccuredBroadcaster
	{
		public static event Action<MentalBreakDef, Pawn> MentalBreakStarted;

		public static void Notify(MentalBreakDef def, Pawn pawn)
		{
			MentalBreakStarted?.Invoke(def, pawn);
		}
	}
}