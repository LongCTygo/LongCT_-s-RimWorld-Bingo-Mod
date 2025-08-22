using RimWorld;
using RimWorldBingoMod.Broadcasters.Anomaly;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorldBingoMod.Objectives.Anomaly
{
	public class SpendBioferriteProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			PsychicRitualCompletedBroadcaster.RitualCompleted += OnRitualCompleted;
		}

		protected override void UnregisterFromBroadcaster()
		{
			PsychicRitualCompletedBroadcaster.RitualCompleted -= OnRitualCompleted;
		}

		private void OnRitualCompleted(PsychicRitual ritual)
		{
			if (ritual == null)
			{
				return;
			}
			//Only people of colony
			if (ritual.lord.ownedPawns.Any((p) => !p.IsColonist))
			{
				return;
			}
			var def = ritual.def;
			if (def is PsychicRitualDef_InvocationCircle ritualDef)
			{
				//Note: All vanilla rituals have only either Shard or Bioferrite. This assumes that is the case.
				//If this is ever changed in the future, a transplier is needed in the patch method to count specifically Bioferrite being consumed.
				//That is way above my pay grade.
				if (
					(ritualDef.RequiredOffering.IsFixedIngredient && ritualDef.RequiredOffering.FixedIngredient == ThingDefOf.Bioferrite)
					|| (ritualDef.RequiredOffering.filter.Allows(ThingDefOf.Bioferrite))
					)
				{
					Achieve((int)ritualDef.RequiredOffering.GetBaseCount());
				}
			}
		}
	}
}