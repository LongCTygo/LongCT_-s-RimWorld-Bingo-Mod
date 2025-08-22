using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class MapGeneratedBroadcaster
	{
		public static event Action<Map> Generated;

		public static void Notify(Map map)
		{
			Generated?.Invoke(map);
		}
	}
}