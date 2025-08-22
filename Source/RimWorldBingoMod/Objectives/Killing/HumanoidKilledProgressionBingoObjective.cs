using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Objectives.Killing
{
	public class HumanoidKilledProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			PawnKilledBroadcaster.Killed += OnKilled;
		}

		protected override void UnregisterFromBroadcaster()
		{
			PawnKilledBroadcaster.Killed -= OnKilled;
		}

		private void OnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (dinfo == null)
			{
				return;
			}
			var damageInfo = dinfo.Value;
			if (damageInfo.Instigator == null || damageInfo.Instigator.Faction != Faction.OfPlayer || damageInfo.Instigator.def != ThingDefOf.Human)
			{
				return;
			}
			//TODO: Use customizable target (animal, mechs, entities, specific xenotypes,...)
			if (pawn.def != ThingDefOf.Human)
			{
				return;
			}
			Achieve();
		}
	}
}