using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Objectives.Research
{
	public class ResearchProjectBingoObjective : BingoObjective
	{
		public ResearchProjectDef requiredResearch;

		public override TaggedString Title => def.title.Translate(requiredResearch.LabelCap);

		public override TaggedString Details => def.details.Translate(requiredResearch.LabelCap);

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			GetRandomDefIfNull(ref requiredResearch);
		}

		public override bool IsConditionAlreadyAchieved()
		{
			return requiredResearch != null && requiredResearch.IsFinished;
		}

		public override bool IsObjectiveSimilar(BingoObjective objective)
		{
			if (objective is ResearchProjectBingoObjective o)
			{
				return requiredResearch == o.requiredResearch;
			}
			return false;
		}

		protected override void RegisterToBroadcaster()
		{
			ResearchCompletionBroadcaster.ResearchCompleted += OnResearchCompletion;
		}

		protected override void UnregisterFromBroadcaster()
		{
			ResearchCompletionBroadcaster.ResearchCompleted -= OnResearchCompletion;
		}

		public void OnResearchCompletion(ResearchProjectDef research)
		{
			if (research == requiredResearch)
			{
				Achieve();
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look(ref requiredResearch, "requiredResearch");
		}
	}
}