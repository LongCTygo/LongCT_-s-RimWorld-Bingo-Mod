using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class ThingProducedBroadcaster
	{
		public static event Action<Thing> Produced;

		public static void Notify(Thing product)
		{
			Produced?.Invoke(product);
		}
	}
}