using System;

namespace RimWorldBingoMod.Broadcasters
{
	public static class GreatestPopulationBroadcaster
	{
		public static event Action<int> Updated;

		public static void Notify(int amount)
		{
			Updated?.Invoke(amount);
		}
	}
}