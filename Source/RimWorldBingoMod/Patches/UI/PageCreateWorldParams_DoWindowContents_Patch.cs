using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Utils;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace RimWorldBingoMod.Patches.UI
{
	[HarmonyPatch(typeof(Page_CreateWorldParams), nameof(Page_CreateWorldParams.DoWindowContents))]
	public class PageCreateWorldParams_DoWindowContents_Patch
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{
			var codes = new List<CodeInstruction>(instructions);
			var endGroup = AccessTools.Method(typeof(Widgets), nameof(Widgets.EndGroup));
			var inject = AccessTools.Method(
				typeof(BingoUIUtils),
				nameof(BingoUIUtils.DoBingoSettingsMenu),
				new[] { typeof(float).MakeByRefType(), typeof(float) } // exact signature
			);

			if (inject == null)
			{
				Log.Error("[Bingo] Could not find DoBingoSettingsMenu(ref float, float)");
				return codes;
			}

			// find the EndGroup() call
			int endIdx = codes.FindIndex(ci => ci.Calls(endGroup));
			if (endIdx < 0)
			{
				Log.Error("[Bingo] Could not find Widgets.EndGroup()");
				return codes;
			}

			// find the brfalse that targets the EndGroup instruction (search backwards)
			int branchIdx = -1;
			for (int i = endIdx - 1; i >= 0; i--)
			{
				var op = codes[i].opcode;
				if (op == OpCodes.Brfalse || op == OpCodes.Brfalse_S)
				{
					var operand = codes[i].operand;

					// operand might directly be the target CodeInstruction
					if (operand is CodeInstruction ci && ci == codes[endIdx])
					{
						branchIdx = i;
						break;
					}

					// operand might be a Label that is attached to the target instruction
					if (operand is Label lbl && codes[endIdx].labels != null && codes[endIdx].labels.Contains(lbl))
					{
						branchIdx = i;
						break;
					}

					// fallback: sometimes operand may be boxed or equal by reference
					if (object.ReferenceEquals(operand, codes[endIdx]))
					{
						branchIdx = i;
						break;
					}
				}
			}

			// Define a new label to jump to (the first injected instruction will carry this label)
			var targetLabel = il.DefineLabel();

			// Create injected instructions; attach the label to the first one
			var ldloca = new CodeInstruction(OpCodes.Ldloca_S, 7);
			ldloca.labels.Add(targetLabel);
			var ldloc = new CodeInstruction(OpCodes.Ldloc_S, 8);
			var call = new CodeInstruction(OpCodes.Call, inject);

			// Insert them right before EndGroup()
			codes.Insert(endIdx, ldloca);
			codes.Insert(endIdx + 1, ldloc);
			codes.Insert(endIdx + 2, call);

			// If we found the branch instruction, retarget its operand to our new label
			if (branchIdx >= 0)
			{
				codes[branchIdx].operand = targetLabel;
			}
			else
			{
				Log.Warning("[Bingo] brfalse -> EndGroup branch not found; injected code may be skipped on some paths.");
			}

			return codes;
		}
	}
}