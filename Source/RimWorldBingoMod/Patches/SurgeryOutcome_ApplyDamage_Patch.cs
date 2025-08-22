using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(SurgeryOutcome), "ApplyDamage")]
	public class SurgeryOutcome_ApplyDamage_Patch
	{
		public static void Postfix(SurgeryOutcome __instance, Pawn patient, BodyPartRecord part)
		{
			SurgeryDealDamageBroadcaster.Notify(patient, __instance.totalDamage);
		}
	}
}