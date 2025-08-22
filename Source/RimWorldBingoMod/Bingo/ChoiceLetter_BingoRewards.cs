using RimWorld;
using RimWorldBingoMod.Rewards;
using RimWorldBingoMod.Utils;
using System.Collections.Generic;
using Verse;

namespace RimWorldBingoMod.Bingo
{
	public class ChoiceLetter_BingoRewards : ChoiceLetter
	{
		public int choices = 3;
		public Map MapToUse => Find.AnyPlayerHomeMap;
		public override bool CanDismissWithRightClick => false;

		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				List<DiaOption> options = new List<DiaOption>();
				if (ArchivedOnly)
				{
					options.Add(Option_Close);
					return options;
				}
				//I want power
				DiaOption powerOption = new DiaOption("Bingo.Letter.RewardsPower".Translate());
				powerOption.action = delegate
				{
					WantPower();
					Find.LetterStack.RemoveLetter(this);
				};
				powerOption.resolveTree = true;
				options.Add(powerOption);
				//I want life
				DiaOption lifeOption = new DiaOption("Bingo.Letter.RewardsLife".Translate());
				lifeOption.action = delegate
				{
					WantLife();
					Find.LetterStack.RemoveLetter(this);
				};
				lifeOption.resolveTree = true;
				options.Add(lifeOption);
				//I want death
				DiaOption deathOption = new DiaOption("Bingo.Letter.RewardsDeath".Translate());
				deathOption.action = delegate
				{
					WantDeath();
					Find.LetterStack.RemoveLetter(this);
				};
				deathOption.resolveTree = true;
				options.Add(deathOption);
				//I want strength
				if (choices >= 4)
				{
					DiaOption strengthOption = new DiaOption("Bingo.Letter.RewardsStrength".Translate());
					strengthOption.action = delegate
					{
						WantStrength();
						Find.LetterStack.RemoveLetter(this);
					};
					strengthOption.resolveTree = true;
					options.Add(strengthOption);
				}
				//I want you
				if (choices >= 5)
				{
					DiaOption pawnOption = new DiaOption("Bingo.Letter.RewardsPawn".Translate());
					pawnOption.action = delegate
					{
						WantPawn();
						Find.LetterStack.RemoveLetter(this);
					};
					pawnOption.resolveTree = true;
					options.Add(pawnOption);
				}
				if (MapToUse == null)
				{
					foreach (var option in options)
					{
						option.Disable("CannotAcceptQuestNoMap".Translate());
					}
					//I seek none (anti-softlock)
					DiaOption noneOption = new DiaOption("Bingo.Letter.RewardsNone".Translate());
					noneOption.action = delegate
					{
						Find.LetterStack.RemoveLetter(this);
					};
					noneOption.resolveTree = true;
					options.Add(noneOption);
				}
				options.Add(Option_Postpone);
				return options;
			}
		}

		//Probably should use PawnKindDef for this, but eh
		private void WantPawn()
		{
			var rewardsDef = DefDatabase<BingoRewardsDef>.GetNamed("SeekPawn");
			var things = rewardsDef.GetRewards();
			BingoRewardsUtils.SendRewards(things, MapToUse);
		}

		private void WantStrength()
		{
			var rewardsDef = DefDatabase<BingoRewardsDef>.GetNamed("SeekStrength");
			var things = rewardsDef.GetRewards();
			BingoRewardsUtils.SendRewards(things, MapToUse);
		}

		private void WantDeath()
		{
			var rewardsDef = DefDatabase<BingoRewardsDef>.GetNamed("SeekDeath");
			var things = rewardsDef.GetRewards();
			BingoRewardsUtils.SendRewards(things, MapToUse);
		}

		private void WantLife()
		{
			var rewardsDef = DefDatabase<BingoRewardsDef>.GetNamed("SeekLife");
			var things = rewardsDef.GetRewards();
			BingoRewardsUtils.SendRewards(things, MapToUse);
		}

		private void WantPower()
		{
			var rewardsDef = DefDatabase<BingoRewardsDef>.GetNamed("SeekPower");
			var things = rewardsDef.GetRewards();
			BingoRewardsUtils.SendRewards(things, MapToUse);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref choices, "choices", 3);
		}
	}
}