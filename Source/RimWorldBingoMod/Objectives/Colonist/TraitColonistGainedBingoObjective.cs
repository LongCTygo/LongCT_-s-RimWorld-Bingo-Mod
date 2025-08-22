using RimWorld;
using RimWorldBingoMod.Broadcasters;
using System.Linq;
using Verse;

namespace RimWorldBingoMod.Objectives.Colonist
{
	public class TraitColonistGainedBingoObjective : BingoObjective
	{
		public TraitDef traitDef;
		public int degree = 0;
		public override TaggedString Title => def.title.Translate(TraitLabel);

		public override TaggedString Details => def.details.Translate(TraitLabel);

		private string TraitLabel => traitDef.DataAtDegree(degree).LabelCap;

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			GetRandomDefIfNull(ref traitDef);
			degree = randomCandidate.degree;
		}

		protected override void RegisterToBroadcaster()
		{
			ColonistRecruitedBroadcaster.Recruited += OnRecruited;
		}

		protected override void UnregisterFromBroadcaster()
		{
			ColonistRecruitedBroadcaster.Recruited -= OnRecruited;
		}

		public override bool IsObjectiveSimilar(BingoObjective objective)
		{
			if (objective is TraitColonistGainedBingoObjective o)
			{
				return traitDef == o.traitDef;
			}
			return false;
		}

		private void OnRecruited(Pawn pawn)
		{
			var traits = pawn.story.traits.allTraits;
			if (traits.Any((t) => IsTrait(t)))
			{
				Achieve();
			}
		}

		private bool IsTrait(Trait t)
		{
			if (t.def != traitDef)
			{
				return false;
			}
			if (degree == 0)
			{
				return true;
			}
			return t.Degree == degree;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look(ref traitDef, "traitDef");
			Scribe_Values.Look(ref degree, "degree");
		}
	}
}