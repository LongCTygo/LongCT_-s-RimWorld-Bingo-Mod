using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class FermentingBarrelTakenOutBroadcaster
	{
		public static event Action<Thing> TakenOut;

		public static void Notify(Thing beer)
		{
			TakenOut?.Invoke(beer);
		}
	}
}