using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorldBingoMod.Rewards
{
	public class BingoRewardsDef : Def
	{
		public List<BingoRewards> rewardsPool = new List<BingoRewards>();
		public List<BingoRewards> additionalRewardsWithoutOdyssey = new List<BingoRewards>();

		public bool rewardIsBionicPawn = false;
		public bool giveAllRewards = false;
		public int randomRewardsBaseAmount = 1;
		public bool makeLegendaryIfPossible = false;

		public List<Thing> GetRewards()
		{
			var rewards = new List<Thing>();
			if (rewardIsBionicPawn)
			{
				Pawn bionicPawn = GetBionicPawn();
				rewards.Add(bionicPawn);
				return rewards;
			}
			var rewardsList = rewardsPool.ToList();
			if (!ModsConfig.OdysseyActive)
			{
				rewardsList.AddRange(additionalRewardsWithoutOdyssey);
			}
			if (!giveAllRewards)
			{
				rewardsList = rewardsList.OrderBy((c) => Rand.Value).Take(randomRewardsBaseAmount).ToList();
			}
			foreach (var bingoRewards in rewardsList)
			{
				var count = bingoRewards.count;
				do
				{
					var reward = ThingMaker.MakeThing(bingoRewards.thingDef);
					reward.stackCount = Math.Min(bingoRewards.thingDef.stackLimit, count);
					count -= reward.stackCount;
					if (makeLegendaryIfPossible && reward is ThingWithComps thingWithComps)
					{
						CompQuality compQuality = thingWithComps.GetComp<CompQuality>();
						if (compQuality != null)
						{
							compQuality.SetQuality(QualityCategory.Legendary, ArtGenerationContext.Outsider);
						}
					}
					if (bingoRewards.thingDef.Minifiable)
					{
						reward = reward.MakeMinified();
					}
					rewards.Add(reward);
				} while (count > 0);
			}
			return rewards;
		}

		private Pawn GetBionicPawn()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(
				PawnKindDefOf.Colonist
				)
			{
				Faction = Faction.OfPlayer,
				ForceGenerateNewPawn = true,
				AllowDead = false,
				Context = PawnGenerationContext.NonPlayer,
				MustBeCapableOfViolence = true,
				CanGeneratePawnRelations = false,
				FixedBiologicalAge = 18,
				//FixedChronologicalAge = 0, //Does not work due to game code
			};

			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.ageTracker.AgeChronologicalTicks = 0; //Manual set
			if (pawn.story != null)
			{
				pawn.story.Childhood = DefDatabase<BackstoryDef>.GetNamedSilentFail("ArchotechCreation");
				pawn.story.Adulthood = null;
			}
			//Clear all hediffs
			pawn.health.RemoveAllHediffs();
			var parts = pawn.RaceProps.body.AllParts.ToList();

			foreach (var part in parts)
			{
				if (pawn.health.hediffSet.HasMissingPartFor(part))
				{
					continue;
				}
				var recipe = DefDatabase<RecipeDef>.AllDefs
					.Where(r => r.addsHediff != null && r.appliedOnFixedBodyParts.Contains(part.def))
					.FirstOrDefault(r => r.addsHediff.defName.ToLower().Contains("bionic"));
				if (recipe != null)
				{
					pawn.health.AddHediff(recipe.addsHediff, part);
				}
			}
			//Force skill level
			foreach (var skill in pawn.skills.skills)
			{
				skill.Level = Math.Max(skill.levelInt, 10);
			}
			//Force trait
			pawn.story.traits.allTraits.Clear();
			var traits = new List<(string defName, int degree)>()
			{
				("GreatMemory", 0),
				("Tough",0),
				("NaturalMood",2),
				("Industriousness",2),
				("SpeedOffset",2),
				("Nerves",2),
				("FastLearner",0),
				("QuickSleeper",0),
				("Kind",0),
			};
			var selectedTraits = traits.OrderBy(t => Rand.Value).Take(3).ToList();
			foreach (var t in selectedTraits)
			{
				var def = DefDatabase<TraitDef>.GetNamedSilentFail(t.defName);
				pawn.story.traits.GainTrait(new Trait(def, t.degree));
			}
			return pawn;
		}
	}
}