using RimWorld;
using RimWorldBingoMod.Broadcasters;
using RimWorldBingoMod.Objectives.Extension;
using Verse;

namespace RimWorldBingoMod.Objectives.Production
{
	public class AnimalProductProgressionBingoObjective : ProgressionBingoObjective
	{
		public AnimalProductTypes productType = AnimalProductTypes.None;
		public ObjectiveCandidate<AnimalProductTypes> typeCandidate;
		public override TaggedString Title => def.title.Translate(goal, GetProductTypeName());

		public override TaggedString Details => def.details.Translate(goal, GetProductTypeName());

		private ThingDef Milk => DefDatabase<ThingDef>.GetNamed("Milk");
		private ThingDef Chemfuel => DefDatabase<ThingDef>.GetNamed("Chemfuel");

		protected override void RegisterToBroadcaster()
		{
			AnimalProductProducedBroadcaster.Produced += OnProduced;
		}

		protected override void UnregisterFromBroadcaster()
		{
			AnimalProductProducedBroadcaster.Produced -= OnProduced;
		}

		public override bool IsObjectiveSimilar(BingoObjective objective)
		{
			if (objective is AnimalProductProgressionBingoObjective o)
			{
				return productType == o.productType;
			}
			return false;
		}

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			var extension = def.GetModExtension<BingoObjectiveExtension>();
			if (extension != null)
			{
				if (productType == AnimalProductTypes.None)
				{
					GetRandomCandidate(ref typeCandidate, extension.animalProductCandidates);
					productType = typeCandidate.value;
				}
			}
			else
			{
				Log.Error("Failed to load mandatory extension for productType.");
			}
		}

		public override void PostInitialize()
		{
			base.PostInitialize();
			ApplyMultiplier(typeCandidate);
		}

		public void OnProduced(Thing thing)
		{
			if (IsThingOfType(thing))
			{
				Achieve(thing.stackCount);
			}
		}

		private bool IsThingOfType(Thing thing)
		{
			switch (productType)
			{
				case AnimalProductTypes.Egg:
					return thing.def.IsEgg;

				case AnimalProductTypes.Milk:
					return thing.def == Milk;

				case AnimalProductTypes.Wool:
					return thing.def.IsWool;

				case AnimalProductTypes.Chemfuel:
					return thing.def == Chemfuel;
			}
			return false;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref productType, "productType");
		}

		private string GetProductTypeName()
		{
			switch (productType)
			{
				case AnimalProductTypes.Egg:
					return "BingoObjective.Animal.Egg".Translate();

				case AnimalProductTypes.Milk:
					return "BingoObjective.Animal.Milk".Translate();

				case AnimalProductTypes.Wool:
					return "BingoObjective.Animal.Wool".Translate();

				case AnimalProductTypes.Chemfuel:
					return "BingoObjective.Animal.Chemfuel".Translate();
			}
			return "ERR:NONE";
		}
	}

	public enum AnimalProductTypes
	{
		None = -1,
		Egg,
		Milk,
		Wool,
		Chemfuel
	}
}