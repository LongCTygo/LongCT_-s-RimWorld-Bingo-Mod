using RimWorld;
using RimWorldBingoMod.Broadcasters;
using RimWorldBingoMod.Objectives.Extension;
using Verse;

namespace RimWorldBingoMod.Objectives.Raiding
{
	public class RaidingFactionBingoObjective : BingoObjective
	{
		public TechLevel? techLevel;
		public override TaggedString Title => def.title.Translate(techLevel.Value.ToStringHuman());

		public override TaggedString Details => def.details.Translate(techLevel.Value.ToStringHuman());

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			Log.Message($"techLevel before = {techLevel}");
			var extension = def.GetModExtension<BingoObjectiveExtension>();
			if (extension != null)
			{
				if (techLevel == null)
				{
					ObjectiveCandidate<TechLevel> candidate = null;
					GetRandomCandidate(ref candidate, extension.techLevelCandidates);
					techLevel = candidate.value;
				}
			}
			else
			{
				Log.Error("Failed to load mandatory extension for techLevel.");
			}
			Log.Message($"techLevel after = {techLevel}");
		}

		public override bool IsObjectiveSimilar(BingoObjective objective)
		{
			if (objective is RaidingFactionBingoObjective o)
			{
				return techLevel == o.techLevel;
			}
			return false;
		}

		protected override void RegisterToBroadcaster()
		{
			FactionSettlementDestroyedBroadcaster.Destroyed += OnDestroyed;
		}

		protected override void UnregisterFromBroadcaster()
		{
			FactionSettlementDestroyedBroadcaster.Destroyed -= OnDestroyed;
		}

		private void OnDestroyed(Faction faction)
		{
			if (faction.def.techLevel == techLevel)
			{
				Achieve();
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref techLevel, "techLevel");
		}
	}
}