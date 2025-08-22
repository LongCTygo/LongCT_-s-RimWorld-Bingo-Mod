using RimWorldBingoMod.Broadcasters;
using RimWorldBingoMod.Objectives.Extension;
using Verse;

namespace RimWorldBingoMod.Objectives.Quest
{
	public class QuestProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			QuestCompletionBroadcaster.QuestCompleted += OnQuestCompleted;
		}

		protected override void UnregisterFromBroadcaster()
		{
			QuestCompletionBroadcaster.QuestCompleted -= OnQuestCompleted;
		}

		public void OnQuestCompleted(RimWorld.Quest quest)
		{
			var def = quest.root;
			var extension = def.GetModExtension<QuestIgnoreModExtension>();
			if (extension != null && extension.ignore)
			{
				return;
			}
			Achieve();
		}
	}
}