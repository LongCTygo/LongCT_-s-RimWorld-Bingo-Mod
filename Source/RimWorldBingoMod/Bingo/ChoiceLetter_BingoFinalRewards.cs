using RimWorld;
using RimWorldBingoMod.Rewards;
using RimWorldBingoMod.Utils;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorldBingoMod.Bingo
{
	public class ChoiceLetter_BingoFinalRewards : ChoiceLetter
	{
		public Map MapToUse => Find.AnyPlayerHomeMap;
		public override bool CanDismissWithRightClick => false;
		public bool doEnding;

		public ChoiceLetter_BingoFinalRewards()
		{
		}

		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				DiaOption treasureOption = new DiaOption("Bingo.Letter.FinalRewardsTreasure".Translate());
				treasureOption.action = delegate
				{
					SeekTreasureFinal();
					StopPlanetkiller();
					BingoBoardComponent.Instance.canPrestige = true;
					if (doEnding)
					{
						BingoBoardComponent.Instance.DoEnding();
					}
					Find.LetterStack.RemoveLetter(this);
				};
				treasureOption.resolveTree = true;
				yield return treasureOption;
				yield return Option_Postpone;
			}
		}

		private void StopPlanetkiller()
		{
			var stopped = StopPlanetkillerOfManager(Find.World.GameConditionManager);
			foreach (var map in Find.Maps)
			{
				if (map != null)
				{
					stopped = StopPlanetkillerOfManager(map.gameConditionManager) || stopped;
				}
			}
			if (stopped)
			{
				Find.LetterStack.ReceiveLetter("Bingo.PlanetkillerStopped.label".Translate(),
					"Bingo.PlanetkillerStopped.text".Translate(), LetterDefOf.PositiveEvent);
				var history = DefDatabase<HistoryEventDef>.GetNamedSilentFail("PlanetkillerStopped");
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (faction != Faction.OfPlayer && faction.def.humanlikeFaction && !faction.def.permanentEnemy)
					{
						faction.TryAffectGoodwillWith(Faction.OfPlayer, 50, canSendMessage: true, canSendHostilityLetter: true, history);
					}
				}
			}
		}

		private bool StopPlanetkillerOfManager(GameConditionManager manager)
		{
			if (manager == null) return false;
			foreach (var condition in manager.ActiveConditions)
			{
				Log.Message($"{condition.LabelCap} of type {condition.GetType().Name}");
			}
			var planetkillers = manager.ActiveConditions
				.Where((c) => c is GameCondition_Planetkiller)
				.ToList();
			if (planetkillers.Empty())
			{
				return false;
			}
			foreach (var planetkiller in planetkillers)
			{
				manager.OnConditionEnd(planetkiller);
			}
			return true;
		}

		private void SeekTreasureFinal()
		{
			var rewardsDef = DefDatabase<BingoRewardsDef>.GetNamed("FinalRewards");
			var things = rewardsDef.GetRewards();
			BingoRewardsUtils.SendRewards(things, MapToUse, true, !doEnding);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref doEnding, "doEnding", false);
		}
	}
}