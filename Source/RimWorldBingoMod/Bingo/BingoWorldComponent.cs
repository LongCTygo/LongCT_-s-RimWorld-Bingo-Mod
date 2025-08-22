using RimWorld.Planet;
using RimWorldBingoMod.Objectives;
using System.Collections.Generic;
using Verse;

namespace RimWorldBingoMod.Bingo
{
	public class BingoWorldComponent : WorldComponent
	{
		public BingoMode bingoMode = BingoMode.Random;
		public BingoBoardDifficulty difficulty = BingoBoardDifficulty.Medium;
		public List<BingoObjective> tempBoard = new List<BingoObjective>();
		public BingoCenterMode centerMode = BingoCenterMode.HarderThanOthers;

		public bool CreateBoardAtStart => bingoMode == BingoMode.Random;
		public bool IsCustom => bingoMode == BingoMode.Custom;
		public bool IsEnabled => bingoMode != BingoMode.Disabled;

		public BingoWorldComponent(World world) : base(world)
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref bingoMode, "bingoMode", BingoMode.Random);
			Scribe_Values.Look(ref difficulty, "difficulty", BingoBoardDifficulty.Medium);
			Scribe_Values.Look(ref centerMode, "centerMode", BingoCenterMode.HarderThanOthers);
		}

		public override void FinalizeInit(bool fromLoad)
		{
			if (!fromLoad)
			{
				bingoMode = BingoInitData.Instance.bingoMode;
				difficulty = BingoInitData.Instance.difficulty;
				centerMode = BingoInitData.Instance.centerMode;
				if (bingoMode == BingoMode.Custom)
				{
					tempBoard = BingoInitData.Instance.customBoard.board;
				}
				BingoInitData.ClearInstance();
			}
		}
	}
}