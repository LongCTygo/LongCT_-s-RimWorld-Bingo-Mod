using RimWorldBingoMod.Broadcasters;
using RimWorldBingoMod.Objectives.Extension;
using Verse;

namespace RimWorldBingoMod.Objectives.Mood
{
	public class MentalBreakProgressionBingoObjective : ProgressionBingoObjective
	{
		public MentalBreakIntensity intensity = MentalBreakIntensity.None;
		public ObjectiveCandidate<MentalBreakIntensity> intensityCandidate;
		public override TaggedString Title => def.title.Translate(goal, Intensity);

		public override TaggedString Details => def.details.Translate(goal, Intensity);
		public TaggedString Intensity => intensity == MentalBreakIntensity.None ? string.Empty : intensity.ToStringSafe() + " ";

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			var extension = def.GetModExtension<BingoObjectiveExtension>();
			if (extension != null && extension.considerIntensity && extension.mentalBreakIntensityCandidates != null)
			{
				GetRandomCandidate(ref intensityCandidate, extension.mentalBreakIntensityCandidates);
				intensity = intensityCandidate.value;
			}
		}

		public override void PostInitialize()
		{
			base.PostInitialize();
			ApplyMultiplier(intensityCandidate);
		}

		protected override void RegisterToBroadcaster()
		{
			MentalBreakOccuredBroadcaster.MentalBreakStarted += OnMentalBreak;
		}

		protected override void UnregisterFromBroadcaster()
		{
			MentalBreakOccuredBroadcaster.MentalBreakStarted -= OnMentalBreak;
		}

		private void OnMentalBreak(MentalBreakDef def, Pawn pawn)
		{
			if (!pawn.IsColonist)
			{
				return;
			}
			if (intensity != MentalBreakIntensity.None && intensity != def.intensity)
			{
				return;
			}
			Achieve();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref intensity, "intensity");
		}
	}
}