using System;

namespace RimWorldBingoMod.Broadcasters.Biotech
{
	public static class WastepackDumpedBroadcaster
	{
		public static event Action<int> Dumped;

		public static void Notify(int amount)
		{
			Dumped?.Invoke(amount);
		}
	}
}