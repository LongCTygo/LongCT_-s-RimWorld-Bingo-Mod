using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.Notify_PlantHarvested))]
	public class QuestManager_NotifyPlantHarvested_Patch
	{
		public static void Postfix(Pawn worker, Thing harvested)
		{
			ThingDef def = harvested.def;
			PlantHarvestedBroadcaster.Notify(def, harvested.stackCount);
		}
	}
}