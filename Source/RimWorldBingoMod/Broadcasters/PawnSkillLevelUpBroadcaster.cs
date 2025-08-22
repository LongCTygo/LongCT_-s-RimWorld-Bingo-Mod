using RimWorld;
using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class PawnSkillLevelUpBroadcaster
	{
		public static event Action<SkillRecord, Pawn> LeveledUp;

		public static void Notify(SkillRecord skillRecord, Pawn pawn)
		{
			LeveledUp?.Invoke(skillRecord, pawn);
		}
	}
}