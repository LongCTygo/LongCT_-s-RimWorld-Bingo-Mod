using System;

namespace RimWorldBingoMod.Broadcasters
{
	public static class CaravanArrivedAtShipBroadcaster
	{
		public static event Action Arrived;

		public static void Notify()
		{
			Arrived?.Invoke();
		}
	}
}