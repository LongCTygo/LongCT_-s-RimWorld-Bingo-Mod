using System;

namespace RimWorldBingoMod.Broadcasters.Odyssey
{
	public static class FishFishedBroadcaster
	{
		public static event Action<int> Fished;

		public static void Notify(int amount)
		{
			Fished?.Invoke(amount);
		}
	}
}