using RimWorld;
using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Objectives.PlantHarvest
{
	public class PlantYieldProgressionBingoObjective : ProgressionBingoObjective
	{
		public ThingDef harvestThingDef;
		public override TaggedString Title => def.title.Translate(goal, harvestThingDef.LabelCap);

		public override TaggedString Details => def.details.Translate(goal, harvestThingDef.LabelCap);

		protected override void RegisterToBroadcaster()
		{
			PlantHarvestedBroadcaster.Harvested += OnHarvest;
		}

		protected override void UnregisterFromBroadcaster()
		{
			PlantHarvestedBroadcaster.Harvested -= OnHarvest;
		}

		public override bool IsObjectiveSimilar(BingoObjective objective)
		{
			if (objective is PlantYieldProgressionBingoObjective o)
			{
				return harvestThingDef == o.harvestThingDef;
			}
			return false;
		}

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			GetRandomDefIfNull(ref harvestThingDef);
		}

		public void OnHarvest(ThingDef harvestThingDef, int amount)
		{
			if (this.harvestThingDef == harvestThingDef)
			{
				Achieve(amount);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look(ref harvestThingDef, "harvestThingDef");
		}
	}
}