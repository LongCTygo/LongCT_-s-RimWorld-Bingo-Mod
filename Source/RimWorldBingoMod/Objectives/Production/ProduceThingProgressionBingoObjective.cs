using RimWorld;
using RimWorldBingoMod.Broadcasters;
using RimWorldBingoMod.Objectives.Extension;
using Verse;

namespace RimWorldBingoMod.Objectives.Production
{
	public class ProduceThingProgressionBingoObjective : ProgressionBingoObjective
	{
		public ThingDef productDef;
		public ObjectiveCandidate<QualityCategory> qualityCandidate;
		public QualityCategory? quality;
		public ObjectiveCandidate<string> stuffCandidate;
		public ThingDef stuffThingDef;

		public override TaggedString Title
		{
			get
			{
				string qualityString = quality != null ? quality.Value.GetLabel() + "+ " : string.Empty;
				string stuffThingString = stuffThingDef != null ? stuffThingDef.label + " " : string.Empty;
				string thingString = productDef.label;
				return def.title.Translate(goal, qualityString, stuffThingString, thingString);
			}
		}

		public override TaggedString Details
		{
			get
			{
				string qualityString = quality != null ? quality.Value.GetLabel() + "+ " : string.Empty;
				string stuffThingString = stuffThingDef != null ? stuffThingDef.label + " " : string.Empty;
				string thingString = productDef.label;
				return def.details.Translate(goal, qualityString, stuffThingString, thingString);
			}
		}

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			GetRandomDefIfNull(ref productDef);
			var extension = def.GetModExtension<BingoObjectiveExtension>();
			if (extension != null)
			{
				if (extension.considerQuality && quality == null)
				{
					GetRandomCandidate(ref qualityCandidate, extension.qualityCandidates);
					quality = qualityCandidate.value;
				}
				if (extension.candidates != null)
				{
					GetRandomDefIfNull(ref stuffThingDef, ref stuffCandidate, extension.candidates);
				}
			}
		}

		public override void PostInitialize()
		{
			base.PostInitialize();
			ApplyMultiplier(qualityCandidate);
			ApplyMultiplier(stuffCandidate);
		}

		public override bool IsObjectiveSimilar(BingoObjective objective)
		{
			if (objective is ProduceThingProgressionBingoObjective o)
			{
				return o.productDef == productDef
					&& o.qualityCandidate == qualityCandidate
					&& o.stuffThingDef == stuffThingDef;
			}
			return false;
		}

		protected override void RegisterToBroadcaster()
		{
			ThingProducedBroadcaster.Produced += OnProduced;
			FermentingBarrelTakenOutBroadcaster.TakenOut += OnProduced;
		}

		protected override void UnregisterFromBroadcaster()
		{
			ThingProducedBroadcaster.Produced -= OnProduced;
			FermentingBarrelTakenOutBroadcaster.TakenOut -= OnProduced;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look(ref productDef, "productDef");
			Scribe_Defs.Look(ref stuffThingDef, "stuffThingDef");
			Scribe_Values.Look(ref quality, "quality");
		}

		public void OnProduced(Thing product)
		{
			if (IsProductTarget(product))
			{
				Achieve(product.stackCount);
			}
		}

		public virtual bool IsProductTarget(Thing product)
		{
			if (product.def != productDef)
			{
				return false;
			}
			if (quality != null && product.TryGetQuality(out var q) && q < quality)
			{
				return false;
			}
			if (stuffThingDef != null && product.Stuff != stuffThingDef)
			{
				return false;
			}
			return true;
		}
	}
}