using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Objectives.Doctoring
{
	public class SurgeryDealDamageProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			SurgeryDealDamageBroadcaster.DamageDealt += OnDamageDealt;
		}

		protected override void UnregisterFromBroadcaster()
		{
			SurgeryDealDamageBroadcaster.DamageDealt -= OnDamageDealt;
		}

		private void OnDamageDealt(Pawn pawn, int damage)
		{
			if (pawn == null)
			{
				return;
			}
			Achieve(damage);
		}
	}
}