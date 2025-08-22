using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class MineableMinedBroadcaster
	{
		public static event Action<Thing> Mined;

		public static void Notify(Thing minedDrop)
		{
			Mined?.Invoke(minedDrop);
		}
	}
}