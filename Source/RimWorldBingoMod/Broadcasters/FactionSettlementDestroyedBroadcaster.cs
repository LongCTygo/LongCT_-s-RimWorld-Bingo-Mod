using RimWorld;
using System;

namespace RimWorldBingoMod.Broadcasters
{
	public static class FactionSettlementDestroyedBroadcaster
	{
		public static event Action<Faction> Destroyed;

		public static void Notify(Faction faction)
		{
			Destroyed?.Invoke(faction);
		}
	}
}