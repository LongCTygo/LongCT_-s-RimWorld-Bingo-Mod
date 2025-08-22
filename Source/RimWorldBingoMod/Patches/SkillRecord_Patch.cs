using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using System.Reflection;
using Verse;

namespace RimWorldBingoMod.Patches
{
	/*
	 * Patch taken from Krafs' Level Up! mod.
	 * https://github.com/krafs/LevelUp/blob/main/Source/Patcher.cs
	 */

	public class SkillRecord_Patch
	{
		internal static void ApplyPatches(Harmony harmony)
		{
			MethodInfo skillRecordLearnOriginal = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn));
			MethodInfo compUseEffectLearnSkillDoEffectOriginal = AccessTools.Method(typeof(CompUseEffect_LearnSkill), nameof(CompUseEffect_LearnSkill.DoEffect));
			HarmonyMethod prefix = new HarmonyMethod(typeof(SkillRecord_Patch), nameof(Prefix));
			HarmonyMethod dirtyAptitudesPostfix = new HarmonyMethod(typeof(SkillRecord_Patch), nameof(DirtyAptitudesPostfix));
			HarmonyMethod learnPostfix = new HarmonyMethod(typeof(SkillRecord_Patch), nameof(LearnPostfix));
			harmony.Patch(skillRecordLearnOriginal, prefix, learnPostfix);
			MethodInfo skillRecordDirtyAptitudesOriginal = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.DirtyAptitudes));
			harmony.Patch(skillRecordDirtyAptitudesOriginal, prefix, dirtyAptitudesPostfix);
		}

		private static void Prefix(out int __state, SkillRecord __instance)
		{
			__state = __instance.Level;
		}

		private static void DirtyAptitudesPostfix(int __state, SkillRecord __instance, Pawn ___pawn)
		{
			if (!___pawn.IsFreeColonist)
			{
				return;
			}

			// DirtyAptitudes can be called on the Create Character-screen if Biotech is used,
			// and either crashes or makes it impossible to move forward.
			// This causes the mod to try and display notifications for a colonist when not yet in a playable program state.
			if (Current.ProgramState == ProgramState.Entry)
			{
				return;
			}

			int previousLevel = __state;
			int currentLevel = __instance.Level;

			if (currentLevel > previousLevel)
			{
				PawnSkillLevelUpBroadcaster.Notify(__instance, ___pawn);
			}
		}

		private static void LearnPostfix(int __state, SkillRecord __instance, Pawn ___pawn, bool direct)
		{
			if (!___pawn.IsFreeColonist)
			{
				return;
			}

			int previousLevel = __state;
			int currentLevel = __instance.Level;

			if (currentLevel > previousLevel)
			{
				PawnSkillLevelUpBroadcaster.Notify(__instance, ___pawn);
			}
		}
	}
}