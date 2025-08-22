using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class PawnKilledBroadcaster
	{
		public static event Action<Pawn, DamageInfo?> Killed;

		public static void Notify(Pawn killedPawn, DamageInfo? dinfo)
		{
			Killed?.Invoke(killedPawn, dinfo);
		}
	}
}