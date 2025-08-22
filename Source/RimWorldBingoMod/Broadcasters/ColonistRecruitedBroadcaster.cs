using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class ColonistRecruitedBroadcaster
	{
		public static event Action<Pawn> Recruited;

		public static void Notify(Pawn pawn)
		{
			Recruited?.Invoke(pawn);
		}
	}
}