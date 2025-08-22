using HarmonyLib;
using RimWorld.Planet;
using RimWorldBingoMod.Broadcasters;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(CaravanArrivalAction_VisitEscapeShip), nameof(CaravanArrivalAction_VisitEscapeShip.Arrived))]
	public class VisitEscapeShip_Arrived_Patch
	{
		public static void Postfix(Caravan caravan, CaravanArrivalAction_VisitEscapeShip __instance)
		{
			CaravanArrivedAtShipBroadcaster.Notify();
		}
	}
}