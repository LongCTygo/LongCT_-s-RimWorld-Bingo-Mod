using RimWorldBingoMod.Broadcasters;
using Verse;

namespace RimWorldBingoMod.Objectives.Mining
{
	public class MiningProgressionBingoObjective : ProgressionBingoObjective
	{
		public ThingDef mineThingDef;
		public override TaggedString Title => def.title.Translate(goal, mineThingDef.label);

		public override TaggedString Details => def.details.Translate(goal, mineThingDef.label);

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			GetRandomDefIfNull(ref mineThingDef);
		}

		public override bool IsObjectiveSimilar(BingoObjective objective)
		{
			if (objective is MiningProgressionBingoObjective o)
			{
				return mineThingDef == o.mineThingDef;
			}
			return false;
		}

		protected override void RegisterToBroadcaster()
		{
			MineableMinedBroadcaster.Mined += OnMined;
		}

		protected override void UnregisterFromBroadcaster()
		{
			MineableMinedBroadcaster.Mined -= OnMined;
		}

		private void OnMined(Thing thing)
		{
			if (thing?.def == mineThingDef)
			{
				Achieve(thing.stackCount);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look(ref mineThingDef, "mineThingDef");
		}
	}
}