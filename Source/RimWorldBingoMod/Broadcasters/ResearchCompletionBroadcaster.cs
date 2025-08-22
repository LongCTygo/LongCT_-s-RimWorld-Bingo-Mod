using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class ResearchCompletionBroadcaster
	{
		public static event Action<ResearchProjectDef> ResearchCompleted;

		public static void Notify(ResearchProjectDef project)
		{
			ResearchCompleted?.Invoke(project);
		}
	}
}