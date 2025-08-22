using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters.Royalty
{
	public static class TechprintAppliedBroadcaster
	{
		public static event Action<ResearchProjectDef> Applied;

		public static void Notify(ResearchProjectDef def)
		{
			Applied?.Invoke(def);
		}
	}
}