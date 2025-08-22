using RimWorld;
using RimWorldBingoMod.Broadcasters.Royalty;
using Verse;

namespace RimWorldBingoMod.Objectives.Royalty
{
	public class PawnRoyalTitleBingoObjective : BingoObjective
	{
		public RoyalTitleDef royalTitleDef;
		public override TaggedString Title => def.title.Translate(royalTitleDef.LabelCap);

		public override TaggedString Details => def.details.Translate(royalTitleDef.LabelCap);

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			GetRandomDefIfNull(ref royalTitleDef);
		}

		public override bool IsObjectiveSimilar(BingoObjective objective)
		{
			if (objective is PawnRoyalTitleBingoObjective o)
			{
				return royalTitleDef == o.royalTitleDef;
			}
			return false;
		}

		public override bool IsConditionAlreadyAchieved()
		{
			foreach (var colonist in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				var title = colonist.royalty?.GetCurrentTitle(Faction.OfEmpire);
				if (title == royalTitleDef)
				{
					return true;
				}
			}
			return false;
		}

		protected override void RegisterToBroadcaster()
		{
			RoyalTitleGainedBroadcaster.Gained += OnGained;
		}

		protected override void UnregisterFromBroadcaster()
		{
			RoyalTitleGainedBroadcaster.Gained -= OnGained;
		}

		private void OnGained(RoyalTitleDef def)
		{
			if (royalTitleDef == def)
			{
				Achieve();
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look(ref royalTitleDef, "royalTitleDef");
		}
	}
}