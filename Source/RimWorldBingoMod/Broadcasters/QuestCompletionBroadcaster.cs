using RimWorld;
using System;

namespace RimWorldBingoMod.Broadcasters
{
	public static class QuestCompletionBroadcaster
	{
		public static event Action<Quest> QuestCompleted;

		public static void Notify(Quest quest)
		{
			QuestCompleted?.Invoke(quest);
		}
	}
}