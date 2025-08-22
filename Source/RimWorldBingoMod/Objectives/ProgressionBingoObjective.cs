using System;
using Verse;

namespace RimWorldBingoMod.Objectives
{
	public abstract class ProgressionBingoObjective : BingoObjective
	{
		public float progress = 0;
		public int goal = -1;
		private bool loadedDuringInit = true;

		public string GetProgressionString()
		{
			return $"{(int)progress}/{goal}";
		}

		public override bool IsConditionAlreadyAchieved()
		{
			if (progress >= goal)
			{
				progress = goal;
				return true;
			}
			return base.IsConditionAlreadyAchieved();
		}

		public override void Initialize(BingoObjectiveDef def)
		{
			base.Initialize(def);
			if (goal <= 0)
			{
				goal = def.goalRange.RandomInRange;
				loadedDuringInit = false;
			}
		}

		public override void PostInitialize()
		{
			base.PostInitialize();
			ApplyOffset(randomCandidate);
			ApplyMultiplier(randomCandidate);
		}

		public override void PostPostInitialize()
		{
			base.PostPostInitialize();
			if (goal <= 0)
			{
				goal = 1;
			}
		}

		protected void ApplyMultiplier(double mult)
		{
			if (!loadedDuringInit)
			{
				goal = (int)Math.Ceiling(goal * mult);
			}
		}

		protected void ApplyMultiplier<T>(ObjectiveCandidate<T> candidate)
		{
			if (!loadedDuringInit && candidate != null)
			{
				goal = (int)Math.Ceiling(goal * candidate.multiplier);
			}
		}

		protected void ApplyOffset<T>(ObjectiveCandidate<T> candidate)
		{
			if (!loadedDuringInit && candidate != null)
			{
				goal += candidate.offset;
			}
		}

		public override void Achieve()
		{
			Achieve(1);
		}

		public void AchieveSetIgnoreProgress(float n)
		{
			if (progress != n && !isCompleted)
			{
				progress = n;
				if (progress >= goal)
				{
					progress = goal;
					base.Achieve();
				}
			}
		}

		public void AchieveSet(float n)
		{
			if (progress < n && !isCompleted)
			{
				progress = n;
				if (progress >= goal)
				{
					progress = goal;
					base.Achieve();
				}
			}
		}

		public void Achieve(float n)
		{
			if (!isCompleted && n != 0)
			{
				progress += n;
				if (progress >= goal)
				{
					progress = goal;
					base.Achieve();
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref progress, "progress", 0);
			Scribe_Values.Look(ref goal, "goal", 0);
		}

		public override string ToString()
		{
			return $"({GetType().Name}) {Title} - {GetProgressionString()} - isCompleted={isCompleted}";
		}
	}
}