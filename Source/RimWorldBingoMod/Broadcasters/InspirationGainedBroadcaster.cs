using RimWorld;
using System;

namespace RimWorldBingoMod.Broadcasters
{
	public static class InspirationGainedBroadcaster
	{
		public static event Action<InspirationDef> Gained;

		public static void Notify(InspirationDef inspiration)
		{
			Gained.Invoke(inspiration);
		}
	}
}