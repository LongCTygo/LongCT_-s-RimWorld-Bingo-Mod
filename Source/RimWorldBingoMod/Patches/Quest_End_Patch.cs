using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(Quest), nameof(Quest.End))]
	public class Quest_End_Patch
	{
		public static void Postfix(Quest __instance, QuestEndOutcome outcome)
		{
			Log.Message($"{__instance.name}: outcome = {outcome.ToStringSafe()}");
			if (outcome == QuestEndOutcome.Success)
			{
				QuestCompletionBroadcaster.Notify(__instance);
			}
		}
	}
}