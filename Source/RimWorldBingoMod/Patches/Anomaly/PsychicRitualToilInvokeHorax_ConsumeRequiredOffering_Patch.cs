using HarmonyLib;
using RimWorldBingoMod.Broadcasters.Anomaly;
using Verse.AI.Group;

namespace RimWorldBingoMod.Patches.Anomaly
{
	//This is the best method I could find
	[HarmonyPatch(typeof(PsychicRitualToil_InvokeHorax), nameof(PsychicRitualToil_InvokeHorax.ConsumeRequiredOffering))]
	public class PsychicRitualToilInvokeHorax_ConsumeRequiredOffering_Patch
	{
		public static void Postfix(PsychicRitual psychicRitual)
		{
			PsychicRitualCompletedBroadcaster.Notify(psychicRitual);
		}
	}
}