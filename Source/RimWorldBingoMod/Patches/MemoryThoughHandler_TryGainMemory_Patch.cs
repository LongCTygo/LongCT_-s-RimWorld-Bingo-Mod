using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(MemoryThoughtHandler), nameof(MemoryThoughtHandler.TryGainMemory),
		new[] { typeof(Thought_Memory), typeof(Pawn) })]
	public class MemoryThoughHandler_TryGainMemory_Patch
	{
		//Should have a prefix or use transpiler here, but currently no objectives are complex enough to require that
		//This is simpler
		public static void Postfix(MemoryThoughtHandler __instance, Thought_Memory newThought, Pawn otherPawn)
		{
			MemoryGainedBroadcaster.Notify(__instance.pawn, otherPawn, newThought);
		}
	}
}