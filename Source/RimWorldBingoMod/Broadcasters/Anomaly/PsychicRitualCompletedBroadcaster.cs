using System;
using Verse.AI.Group;

namespace RimWorldBingoMod.Broadcasters.Anomaly
{
	public static class PsychicRitualCompletedBroadcaster
	{
		public static event Action<PsychicRitual> RitualCompleted;

		public static void Notify(PsychicRitual ritual)
		{
			RitualCompleted?.Invoke(ritual);
		}
	}
}