using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Objectives.Research
{
	public class ResearchProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

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
			Achieve();
		}
	}
}