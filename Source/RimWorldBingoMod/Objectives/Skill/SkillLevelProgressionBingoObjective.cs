using RimWorld;
using RimWorldBingoMod.Broadcasters;
using System;
using Verse;

namespace RimWorldBingoMod.Objectives.Skill
{
	public class SkillLevelProgressionBingoObjective : ProgressionBingoObjective
	{
		public SkillDef skillDef;
		public override TaggedString Title => def.title.Translate(skillDef.LabelCap, goal);

		public override TaggedString Details => def.details.Translate(skillDef.LabelCap, goal);

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			GetRandomDefIfNull(ref skillDef);
		}

		public override bool IsConditionAlreadyAchieved()
		{
			foreach (var colonist in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				var skill = colonist.skills.GetSkill(skillDef);
				if (skill == null)
				{
					continue;
				}
				progress = Math.Max(progress, skill.Level);
			}
			//Log.Message($"Skill Level ForceCheck: max on {skillDef.LabelCap} is {progress}.");
			return base.IsConditionAlreadyAchieved();
		}

		public override bool IsObjectiveSimilar(BingoObjective objective)
		{
			if (objective is SkillLevelProgressionBingoObjective o)
			{
				return skillDef == o.skillDef;
			}
			return false;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look(ref skillDef, "skillDef");
		}

		protected override void RegisterToBroadcaster()
		{
			PawnSkillLevelUpBroadcaster.LeveledUp += OnLeveledUp;
			ColonistRecruitedBroadcaster.Recruited += OnRecruited;
		}

		protected override void UnregisterFromBroadcaster()
		{
			PawnSkillLevelUpBroadcaster.LeveledUp -= OnLeveledUp;
			ColonistRecruitedBroadcaster.Recruited -= OnRecruited;
		}

		private void OnLeveledUp(SkillRecord record, Pawn pawn)
		{
			if (!pawn.IsColonist)
			{
				return;
			}
			if (record.def != skillDef)
			{
				return;
			}
			if (record.Level > progress)
			{
				AchieveSet(record.Level);
			}
		}

		private void OnRecruited(Pawn pawn)
		{
			SkillRecord skillRecord = pawn.skills.GetSkill(skillDef);
			if (skillRecord == null)
			{
				return;
			}
			if (!pawn.IsColonist)
			{
				return; //Probably not necessary???
			}
			if (skillRecord.Level > progress)
			{
				AchieveSet(skillRecord.Level);
			}
		}
	}
}