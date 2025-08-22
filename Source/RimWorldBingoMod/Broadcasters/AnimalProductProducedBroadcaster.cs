using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class AnimalProductProducedBroadcaster
	{
		public static event Action<Thing> Produced;

		public static void Notify(Thing product)
		{
			//Log.Message($"Produced {product.def.defName} with stack size of {product.stackCount}.");
			Produced?.Invoke(product);
		}
	}
}