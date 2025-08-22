using RimWorld.Planet;
using System;

namespace RimWorldBingoMod.Broadcasters
{
	public static class CaravanArrivedAtTileBroadcaster
	{
		public static event Action<Caravan> Arrived;

		public static void Notify(Caravan caravan)
		{
			Arrived?.Invoke(caravan);
		}
	}
}