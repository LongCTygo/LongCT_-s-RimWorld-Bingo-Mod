using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters.Biotech;

namespace RimWorldBingoMod.Patches.Biotech
{
	//Patching Goodwill so that it works in orbit also
	[HarmonyPatch(typeof(CompDissolutionEffect_Goodwill), nameof(CompDissolutionEffect_Goodwill.DoDissolutionEffectWorld))]
	public class CompDissolutionEffectGoodwill_DoDissolutionEffectWorld_Patch
	{
		public static void Postfix(int amount)
		{
			WastepackDumpedBroadcaster.Notify(amount);
		}
	}
}