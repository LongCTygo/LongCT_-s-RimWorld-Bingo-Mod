using RimWorld;
using System;

namespace RimWorldBingoMod.Broadcasters.Royalty
{
	public static class RoyalTitleGainedBroadcaster
	{
		public static event Action<RoyalTitleDef> Gained;

		public static void Notify(RoyalTitleDef title)
		{
			Gained?.Invoke(title);
		}
	}
}