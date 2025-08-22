using RimWorld;
using RimWorldBingoMod.Bingo;
using RimWorldBingoMod.Utils;
using System.Collections.Generic;
using Verse;

namespace RimWorldBingoMod.Objectives
{
	public abstract class BingoObjective : IExposable
	{
		public BingoObjectiveDef def;
		public ObjectiveCandidate<string> randomCandidate;
		public bool isCompleted = false;
		public abstract TaggedString Title { get; }
		public abstract TaggedString Details { get; }

		private bool isListening = false;

		protected abstract void RegisterToBroadcaster();

		protected abstract void UnregisterFromBroadcaster();

		public void StartListening()
		{
			if (isListening) return;
			isListening = true;
			RegisterToBroadcaster();
		}

		public void StopListening()
		{
			if (!isListening) return;
			isListening = false;
			UnregisterFromBroadcaster();
		}

		public virtual bool IsConditionAlreadyAchieved()
		{
			return isCompleted;
		}

		public virtual bool IsObjectiveSimilar(BingoObjective objective)
		{
			return false;
		}

		public BingoObjective()
		{
		}

		public void ForceCheckAchieved()
		{
			if (isCompleted) return;
			if (IsConditionAlreadyAchieved())
			{
				TryComplete();
			}
		}

		public void TryInitialize(BingoObjectiveDef def)
		{
			Initialize(def);
			PostInitialize();
			PostPostInitialize();
		}

		public virtual void Initialize(BingoObjectiveDef def)
		{
			this.def = def;
		}

		public virtual void PostInitialize()
		{
		}

		public virtual void PostPostInitialize()
		{
		}

		public virtual void ExposeData()
		{
			Scribe_Defs.Look(ref def, "def");
			Scribe_Values.Look(ref isCompleted, "isCompleted", false);
		}

		public virtual void Achieve()
		{
			TryComplete();
		}

		public void TryComplete()
		{
			if (!isCompleted)
			{
				isCompleted = true;
				UnregisterFromBroadcaster();
				SendLetter();
				BingoBoardComponent.Instance.CheckForBingo();
			}
		}

		private void SendLetter()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			string label = "Bingo.Letter.ObjectiveComplete.label".Translate();
			string text = "Bingo.Letter.ObjectiveComplete.text".Translate(
				Title
				);
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent);
		}

		protected void GetRandomDef<T>(ref T d) where T : Def
		{
			GetRandomDef(ref d, def.candidates);
		}

		protected void GetRandomDef<T>(ref T d, IEnumerable<ObjectiveCandidate<string>> candidates) where T : Def
		{
			GetRandomDef(ref d, ref randomCandidate, candidates);
		}

		protected void GetRandomDef<T>(ref T d, ref ObjectiveCandidate<string> candidate, IEnumerable<ObjectiveCandidate<string>> candidates) where T : Def
		{
			GetRandomCandidate(ref candidate, candidates);
			d = DefDatabase<T>.GetNamedSilentFail(candidate.value);
			if (d == null)
			{
				Log.Error($"Missing {nameof(T)}: {candidate.value} for bingo objective {def.defName}");
			}
		}

		protected void GetRandomDefIfNull<T>(ref T d) where T : Def
		{
			if (d == null)
			{
				GetRandomDef(ref d);
			}
		}

		protected void GetRandomDefIfNull<T>(ref T d, IEnumerable<ObjectiveCandidate<string>> candidates) where T : Def
		{
			if (d == null)
			{
				GetRandomDef(ref d, candidates);
			}
		}

		protected void GetRandomDefIfNull<T>(ref T d, ref ObjectiveCandidate<string> candidate, IEnumerable<ObjectiveCandidate<string>> candidates) where T : Def
		{
			if (d == null)
			{
				GetRandomDef(ref d, ref candidate, candidates);
			}
		}

		protected void GetRandomCandidate<T>(ref ObjectiveCandidate<T> candidate, IEnumerable<ObjectiveCandidate<T>> candidates)
		{
			candidate = RandomUtils.RandomWeighted(candidates, (c) => c.weight);
		}

		public override string ToString()
		{
			return $"({GetType().Name}) {Title} - isCompleted={isCompleted}";
		}
	}
}