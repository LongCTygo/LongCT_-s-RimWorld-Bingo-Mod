using RimWorld;
using RimWorld.Planet;
using RimWorldBingoMod.Broadcasters;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorldBingoMod.Objectives.Biomes
{
	public class VisitBiomesProgressionBingoObjective : ProgressionBingoObjective
	{
		public List<BiomeDef> visitedBiomes = new List<BiomeDef>();
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details
		{
			get
			{
				var details = def.details.Translate(goal);
				if (visitedBiomes.Count > 0)
				{
					StringBuilder sb = new StringBuilder(details);
					sb.AppendLine().AppendInNewLine("BingoObjective.AchievedListTitle".Translate());
					foreach (BiomeDef biome in visitedBiomes)
					{
						sb.AppendInNewLine($"- {biome.LabelCap}");
					}
					return sb.ToString();
				}
				return details;
			}
		}

		protected override void RegisterToBroadcaster()
		{
			MapGeneratedBroadcaster.Generated += OnMapGenerated;
			CaravanArrivedAtTileBroadcaster.Arrived += OnCaravanEnterTile;
		}

		protected override void UnregisterFromBroadcaster()
		{
			MapGeneratedBroadcaster.Generated -= OnMapGenerated;
			CaravanArrivedAtTileBroadcaster.Arrived -= OnCaravanEnterTile;
		}

		public override bool IsConditionAlreadyAchieved()
		{
			foreach (var map in Find.Maps)
			{
				CheckIfBiomeIsNew(map.Biomes);
			}
			//NOTE: Perhaps check for caravan as well?
			return base.IsConditionAlreadyAchieved();
		}

		public void OnCaravanEnterTile(Caravan caravan)
		{
			if (caravan.Tile == PlanetTile.Invalid)
			{
				return;
			}
			Tile tile = Find.WorldGrid[caravan.Tile];
			if (tile != null)
			{
				CheckIfBiomeIsNew(tile.Biomes);
			}
		}

		public void OnMapGenerated(Map map)
		{
			if (map.IsPocketMap)
			{
				return;
			}
			if (map.Tile == PlanetTile.Invalid)
			{
				return;
			}
			Tile tile = Find.WorldGrid[map.Tile];
			if (tile != null)
			{
				CheckIfBiomeIsNew(tile.Biomes);
			}
		}

		private void CheckIfBiomeIsNew(IEnumerable<BiomeDef> biomes)
		{
			foreach (var biome in biomes)
			{
				CheckIfBiomeIsNew(biome);
			}
		}

		private void CheckIfBiomeIsNew(BiomeDef biome)
		{
			if (visitedBiomes.Contains(biome))
			{
				return;
			}
			visitedBiomes.Add(biome);
			AchieveSet(visitedBiomes.Count);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look(ref visitedBiomes, "visitedBiomes", LookMode.Def);
		}
	}
}