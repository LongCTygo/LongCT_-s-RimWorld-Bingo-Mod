using HarmonyLib;
using RimWorld.Planet;
using RimWorldBingoMod.Broadcasters;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(Caravan_PathFollower), "TryEnterNextPathTile")]
	public class CaravanPathFollower_TryEnterNextPathTile_Patch
	{
		public static void Postfix(Caravan_PathFollower __instance, Caravan ___caravan)
		{
			CaravanArrivedAtTileBroadcaster.Notify(___caravan);
		}
	}
}