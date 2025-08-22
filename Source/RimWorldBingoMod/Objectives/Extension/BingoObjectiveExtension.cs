using RimWorld;
using RimWorldBingoMod.Objectives.Production;
using System.Collections.Generic;
using Verse;

namespace RimWorldBingoMod.Objectives.Extension
{
	public class BingoObjectiveExtension : DefModExtension
	{
		public List<ObjectiveCandidate<string>> candidates;
		public List<ObjectiveCandidate<QualityCategory>> qualityCandidates;
		public List<ObjectiveCandidate<AnimalProductTypes>> animalProductCandidates;
		public List<ObjectiveCandidate<TechLevel>> techLevelCandidates;
		public List<ObjectiveCandidate<MentalBreakIntensity>> mentalBreakIntensityCandidates;
		public bool considerQuality = false;
		public bool considerIntensity = false;
	}
}