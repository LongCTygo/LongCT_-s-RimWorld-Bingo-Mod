using RimWorld;
using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters.Biotech
{
	public static class XenogermImplantedBroadcaster
	{
		public static event Action<Pawn, Xenogerm> Implanted;

		public static void Notify(Pawn pawn, Xenogerm xenogerm)
		{
			Implanted?.Invoke(pawn, xenogerm);
		}
	}
}