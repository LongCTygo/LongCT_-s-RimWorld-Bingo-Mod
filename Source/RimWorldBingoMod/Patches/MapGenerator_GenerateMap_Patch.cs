using HarmonyLib;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(MapGenerator), nameof(MapGenerator.GenerateMap))]
	public class MapGenerator_GenerateMap_Patch
	{
		public static void Postfix(Map __result)
		{
			MapGeneratedBroadcaster.Notify(__result);
		}
	}
}