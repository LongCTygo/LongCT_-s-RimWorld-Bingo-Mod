using LudeonTK;
using RimWorld;
using RimWorldBingoMod.Extensions;
using RimWorldBingoMod.Objectives;
using RimWorldBingoMod.Utils;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorldBingoMod.Bingo
{
	public class BingoBoardComponent : GameComponent
	{
		public static BingoBoardComponent Instance;

		private bool isActive = false;
		private int closeTick = -1;
		public List<BingoObjective> board = new List<BingoObjective>(25);
		public BingoStatisticsInfo stats;
		public bool hasShownCredits = false;
		public int prestige = 0;
		public bool canPrestige = false;

		public bool IsValid => isActive;

		public BingoBoardComponent(Game game)
		{
			Instance = this;
		}

		public override void FinalizeInit()
		{
			base.FinalizeInit();
			if (stats == null)
			{
				stats = new BingoStatisticsInfo();
			}
			if (board == null)
			{
				isActive = false;
			}
		}

		public override void StartedNewGame()
		{
			Instance = this;
			var worldComp = Find.World.GetComponent<BingoWorldComponent>();
			if (worldComp.CreateBoardAtStart)
			{
				InitializeBoard(worldComp.difficulty, worldComp.centerMode);
				StartListening();
				ForceCheckAll();
			}
			else if (worldComp.IsCustom)
			{
				board = worldComp.tempBoard;
				StartListening();
				ForceCheckAll();
				worldComp.tempBoard = null;
			}
			if (worldComp.IsEnabled)
			{
				isActive = true;
			}
		}

		public override void LoadedGame()
		{
			Instance = this;
			//var worldComp = Find.World.GetComponent<BingoWorldComponent>();
			if (isActive)
			{
				isActive = true;
				StartListening();
				ForceCheckAll();
			}
		}

		public override void GameComponentTick()
		{
			base.GameComponentTick();

			if (isActive && Current.ProgramState != ProgramState.Playing)
			{
				//Log.Message("[BingoMod] Game unloaded, stopping Bingo listeners.");
				Shutdown();
				return;
			}

			if (closeTick > 0 && Find.TickManager.TicksGame == closeTick && !hasShownCredits)
			{
				ShowCredits();
			}
		}

		private void ShowCredits()
		{
			Find.TickManager.Pause();
			hasShownCredits = true;
			closeTick = -1;
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Bingo.Credits".Translate());
			var igts = stats.AllIGTs;
			var rtas = stats.AllRTAs;
			for (int i = 0; i < 12; i++)
			{
				sb.AppendLine("Bingo.Letter.MilestoneRecord".Translate(i + 1, igts[i].ToStringTicksToPeriodVerbose(allowQuadrums: true), rtas[i].ToRealTimeStringDisplay()));
			}
			sb.AppendLine().AppendLine("Bingo.CreditsSelfInsert".Translate());
			var credits = sb.ToString();
			GameVictoryUtility.ShowCredits(credits, SongDefOf.EndCreditsSong);
		}

		public void Prestige()
		{
			prestige++;
			InitializeBoard(BingoBoardDifficulty.Hard, BingoCenterMode.HarderThanOthers, true);
		}

		public void InitializeBoard(BingoBoardDifficulty difficulty = BingoBoardDifficulty.Medium,
			BingoCenterMode centerMode = BingoCenterMode.HarderThanOthers,
			bool isPrestige = false)
		{
			StopListening();
			stats = new BingoStatisticsInfo();
			canPrestige = false;
			board = BingoGeneratorUtils.GenerateNewBoard(difficulty, centerMode, isPrestige);
		}

		private void StartListening()
		{
			foreach (var objective in board)
			{
				if (objective != null && !objective.isCompleted)
					objective.StartListening();
			}
			Log.Message("Started listening for all objectives.");
		}

		private void StopListening()
		{
			foreach (var objective in board)
			{
				if (objective != null)
					objective.StopListening();
			}
		}

		private void ForceCheckAll()
		{
			foreach (var objective in board)
			{
				objective?.ForceCheckAchieved();
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref isActive, "isActive");
			Scribe_Collections.Look(ref board, "board", LookMode.Deep);
			Scribe_Deep.Look(ref stats, "stats");
			Scribe_Values.Look(ref prestige, "prestige", 0);
			Scribe_Values.Look(ref canPrestige, "canPrestige", false);
			Scribe_Values.Look(ref hasShownCredits, "hasShownCredits", false);
			Scribe_Values.Look(ref closeTick, "closeTick", -1);
		}

		public void Shutdown()
		{
			StopListening();
			isActive = false;
			Instance = null;
		}

		[DebugAction("BingoMod", "Print All Objectives", allowedGameStates = AllowedGameStates.Playing)]
		public static void DebugPrintObjectives()
		{
			if (Current.Game == null || !Prefs.DevMode)
			{
				Log.Warning("DebugPrintBoardState called outside of dev mode or without active game");
				return;
			}
			var bingoComp = Current.Game.GetComponent<BingoBoardComponent>();
			if (bingoComp?.board == null)
			{
				Log.Error("No bingo board found");
				return;
			}
			Log.Message("=== Bingo Objectives ===");
			foreach (var item in bingoComp.board)
			{
				Log.Message(item);
			}
		}

		[DebugAction("BingoMod", "Regenerate Bingo Board", allowedGameStates = AllowedGameStates.Playing)]
		public static void DebugGenerateBoard()
		{
			if (Current.Game == null || !Prefs.DevMode)
			{
				Log.Warning("DebugGenerateBoard called outside of dev mode or without active game");
				return;
			}
			Instance.InitializeBoard();
			Instance.StartListening();
			Instance.ForceCheckAll();
		}

		[DebugAction("BingoMod", "Force Complete Objective", allowedGameStates = AllowedGameStates.Playing)]
		public static void DebugForceCompleteObjective()
		{
			if (Current.Game == null || !Prefs.DevMode)
			{
				Log.Warning("[BingoMod] DebugForceCompleteObjective called outside of dev mode or without active game.");
				return;
			}

			var bingoComp = Current.Game.GetComponent<BingoBoardComponent>();
			if (bingoComp?.board == null || bingoComp.board.Count == 0)
			{
				Log.Error("[BingoMod] No bingo board found.");
				return;
			}

			var options = new List<DebugMenuOption>();
			for (int i = 0; i < bingoComp.board.Count; i++)
			{
				var obj = bingoComp.board[i];
				string label = $"{i}: {(obj?.ToString() ?? "NULL")}";
				options.Add(new DebugMenuOption(label, DebugMenuOptionMode.Action, () =>
				{
					if (obj != null)
					{
						Log.Message($"[BingoMod] Forcing completion of objective at index {i}.");
						obj.TryComplete();
					}
					else
					{
						Log.Warning($"[BingoMod] Objective at index {i} is null.");
					}
				}));
			}

			Find.WindowStack.Add(new Dialog_DebugOptionListLister(options));
		}

		public void CheckForBingo()
		{
			int count = 0;
			for (int i = 0; i < 5; i++)
			{
				count += IsBingo(i, 5) ? 1 : 0;
				count += IsBingo(i * 5, 1) ? 1 : 0;
			}
			// Diagonal
			count += IsBingo(0, 6) ? 1 : 0;
			count += IsBingo(4, 4) ? 1 : 0;
			if (count > stats.Bingo)
			{
				int prev = stats.Bingo;
				stats.Bingo = count;
				//Announce
				SendLetterAndRewards(count - prev);
			}
		}

		private void SendLetterAndRewards(int bingos)
		{
			SendLetter();
			SendRewards(bingos);
		}

		private void SendRewards(int bingos)
		{
			var thoughtDefName = bingos == 12 ? "BingoBlackoutMoodBoost" : "BingoMoodBoost";
			var thoughtDef = DefDatabase<ThoughtDef>.GetNamed(thoughtDefName);
			foreach (var pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				if (pawn.needs?.mood != null)
				{
					for (int i = 0; i < bingos; i++)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(thoughtDef);
					}
				}
			}
			//Archotech Rewards
			if (stats.Bingo >= 12)
			{
				Log.Message("final rewards");
				SendFinalRewards();
			}
			else if (stats.rewardsStage < 1 && stats.Bingo >= 2)
			{
				SendSpecialRewards(3);
				stats.rewardsStage++;
			}
			else if (stats.rewardsStage < 2 && stats.Bingo >= 4)
			{
				SendSpecialRewards(4);
				stats.rewardsStage++;
			}
			else if (stats.rewardsStage < 3 && stats.Bingo >= 8)
			{
				SendSpecialRewards(5);
				stats.rewardsStage++;
			}
			//Smart ass way
			//if (stats.Bingo >= stats.rewardsStage * 3 + 2)
			//{
			//	SendSpecialRewards(stats.rewardsStage + 3);
			//	stats.rewardsStage++;
			//}
		}

		private void SendFinalRewards()
		{
			var doEnding = prestige == 0;
			string label, text;
			if (doEnding)
			{
				label = "Bingo.Letter.ArchotechRewardLastCall.label";
				text = "Bingo.Letter.ArchotechRewardLastCall.text";
			}
			else
			{
				label = "Bingo.Letter.ArchotechFinalRewardPrestige.label";
				text = "Bingo.Letter.ArchotechFinalRewardPrestige.text";
			}
			var def = DefDatabase<LetterDef>.GetNamedSilentFail("BingoFinalRewards");
			var letter = LetterMaker.MakeLetter(
				label.Translate(),
				text.Translate(),
				def);
			if (letter is ChoiceLetter_BingoFinalRewards choiceLetter)
			{
				choiceLetter.doEnding = doEnding;
				Find.LetterStack.ReceiveLetter(letter, delayTicks: 2500);
			}
			else
			{
				Log.Error("Failed to send bingo rewards letter - invalid type.");
			}
		}

		private void SendSpecialRewards(int choices)
		{
			string label, text;
			if (prestige == 0)
			{
				label = "Bingo.Letter.ArchotechReward.label";
				text = $"Bingo.Letter.ArchotechReward{choices - 2}.text";
			}
			else
			{
				label = "Bingo.Letter.ArchotechRewardPrestige.label";
				text = "Bingo.Letter.ArchotechRewardPrestige.text";
				choices = 5;
			}
			var def = DefDatabase<LetterDef>.GetNamedSilentFail("BingoRewards");
			var letter = LetterMaker.MakeLetter(
				label.Translate(),
				text.Translate(),
				def);
			if (letter is ChoiceLetter_BingoRewards choiceLetter)
			{
				choiceLetter.choices = choices;
				Find.LetterStack.ReceiveLetter(letter, delayTicks: 2500);
				letter.StartTimeout(62500);
			}
			else
			{
				Log.Error("Failed to send bingo rewards letter - invalid type.");
			}
		}

		private bool IsBingo(int start, int v)
		{
			for (int i = 0; i < 5; i++)
			{
				var objective = board[start + i * v];
				if (objective == null || !objective.isCompleted)
				{
					return false;
				}
			}
			return true;
		}

		private void SendLetter()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (stats.Bingo >= 12)
			{
				SendFinalLetter();
			}
			else
			{
				string label = "Bingo.Letter.BingoComplete.label".Translate();
				string text = "Bingo.Letter.BingoComplete.text".Translate(
					stats.Bingo,
					stats.CurrentIGT.ToStringTicksToPeriodVerbose(allowQuadrums: true),
					stats.CurrentRTA.ToRealTimeStringDisplay()
					);
				//Strange Sign
				if (prestige == 0 && stats.strangeSignStage < 1 && stats.Bingo >= 1)
				{
					text += "\n\n" + "Bingo.Letter.StrangeSign1".Translate();
					stats.strangeSignStage++;
				}
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.RitualOutcomePositive);
			}
		}

		private void SendFinalLetter()
		{
			string label = "Bingo.Letter.BingoBoardComplete.label".Translate();
			string text = "Bingo.Letter.BingoBoardComplete.text".Translate(
				stats.Bingo,
				stats.CurrentIGT.ToStringTicksToPeriodVerbose(allowQuadrums: true),
				stats.CurrentRTA.ToRealTimeStringDisplay()
				);
			if (prestige == 0)
			{
				text += "\n\n" + "Bingo.Letter.StrangeSign2".Translate();
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.RitualOutcomePositive);
		}

		public void DoEnding()
		{
			closeTick = Find.TickManager.TicksGame + 300;
			Find.TickManager.slower.SignalForceNormalSpeed();
			Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(999f, 3f);
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera();
			ScreenFader.StartFade(Color.white, 4.9166665f);
		}
	}
}