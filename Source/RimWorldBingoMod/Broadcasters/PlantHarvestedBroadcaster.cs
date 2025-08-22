using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class PlantHarvestedBroadcaster
	{
		public static event Action<ThingDef, int> Harvested;

		public static void Notify(ThingDef harvestThingDef, int amount)
		{
			Harvested?.Invoke(harvestThingDef, amount);
		}
	}
}