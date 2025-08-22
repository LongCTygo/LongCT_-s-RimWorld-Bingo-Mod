using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(CompHasGatherableBodyResource), nameof(CompHasGatherableBodyResource.Gathered))]
	public class CompHasGatherableBodyResource_Gathered_Patch
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var list = new List<CodeInstruction>(instructions);
			// Verse.Thing::stackCount
			var stackCountField = AccessTools.Field(typeof(Thing), nameof(Thing.stackCount));
			// AnimalProductProducedBroadcaster.Notify(Thing)
			var notifyMethod = AccessTools.Method(typeof(AnimalProductProducedBroadcaster), nameof(AnimalProductProducedBroadcaster.Notify));
			for (int i = 0; i < list.Count; i++)
			{
				yield return list[i];
				if (list[i].opcode == OpCodes.Stfld && Equals(list[i].operand, stackCountField))
				{
					yield return new CodeInstruction(OpCodes.Dup); // duplicate thing
					yield return new CodeInstruction(OpCodes.Call, notifyMethod);
				}
			}
		}
	}
}