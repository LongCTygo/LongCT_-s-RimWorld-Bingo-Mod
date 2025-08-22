using RimWorld;
using RimWorldBingoMod.Bingo;
using RimWorldBingoMod.Files;
using RimWorldBingoMod.Utils;
using UnityEngine;
using Verse;

namespace RimWorldBingoMod.UI
{
	[StaticConstructorOnStartup]
	public class Dialog_BingoBoardCustomize : Window
	{
		public override Vector2 InitialSize => new Vector2(1300f, 750f);
		private static readonly Texture2D BingoTitleTexture = ContentFinder<Texture2D>.Get("BingoIcon");

		public Dialog_BingoBoardCustomize()
		{
			doCloseButton = true;
			forcePause = true;
			absorbInputAroundWindow = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			if (BingoInitData.Instance.customBoard.board.Count != 25)
			{
				BingoGeneratorUtils.PatchBoard(ref BingoInitData.Instance.customBoard.board);
			}
			Rect leftRect = inRect.LeftPart(0.5f);
			Rect rightRect = inRect.RightPart(0.5f);
			DrawLeftRect(leftRect);
			DrawRightRect(rightRect);
		}

		private void DrawRightRect(Rect rightRect)
		{
			var topRect = rightRect.TopPart(0.25f);
			var bottomRect = rightRect.BottomPart(0.75f);
			//Top Rect
			DrawTopRightRect(topRect);
		}

		private void DrawTopRightRect(Rect topRect)
		{
			var listing = new Listing_Standard();
			listing.ColumnWidth = 200f;
			listing.Begin(topRect);
			//Generate (Difficulty)
			Text.Font = GameFont.Medium;
			listing.Label("Bingo.UI.GenerateBoard".Translate());
			Text.Font = GameFont.Small;
			if (listing.ButtonText("Bingo.UI.Generate".Translate("Bingo.UI.Difficulty.Easy".Translate())))
			{
				BingoInitData.Instance.customBoard.board = BingoGeneratorUtils.GenerateNewBoard(BingoBoardDifficulty.Easy,
					BingoInitData.Instance.centerMode);
			}
			if (listing.ButtonText("Bingo.UI.Generate".Translate("Bingo.UI.Difficulty.Medium".Translate())))
			{
				BingoInitData.Instance.customBoard.board = BingoGeneratorUtils.GenerateNewBoard(BingoBoardDifficulty.Medium,
					BingoInitData.Instance.centerMode);
			}
			if (listing.ButtonText("Bingo.UI.Generate".Translate("Bingo.UI.Difficulty.Hard".Translate())))
			{
				BingoInitData.Instance.customBoard.board = BingoGeneratorUtils.GenerateNewBoard(BingoBoardDifficulty.Hard,
					BingoInitData.Instance.centerMode);
			}
			//Center Mode
			listing.NewColumn();
			Text.Font = GameFont.Medium;
			listing.Label("Bingo.UI.CenterMode".Translate());
			Text.Font = GameFont.Small;
			BingoUIUtils.DrawBingoCenterList(listing);
			//Save/Load Presets
			listing.NewColumn();
			if (listing.ButtonText("Bingo.UI.CustomBoard.Load".Translate()))
			{
				Find.WindowStack.Add(new Dialog_BingoBoardList_Load(SetBoard));
			}
			if (listing.ButtonText("Bingo.UI.CustomBoard.Save".Translate()))
			{
				Find.WindowStack.Add(new Dialog_BingoBoardList_Save(BingoInitData.Instance.customBoard));
			}
			if (listing.ButtonText("Bingo.UI.CustomBoard.Open".Translate()))
			{
				Application.OpenURL(GenFilePaths_Bingo.BingoBoardsFolderPath);
			}
			listing.End();
		}

		private void DrawLeftRect(Rect leftRect)
		{
			float titleWidth = 535f;
			Rect titleRect = new Rect(
				leftRect.x + (leftRect.width - titleWidth) / 2f,
				leftRect.y,
				titleWidth,
				80f
			);
			GUI.DrawTexture(titleRect, BingoTitleTexture, ScaleMode.ScaleToFit, true);
			BingoUIUtils.DrawBoard(leftRect, BingoInitData.Instance.customBoard.board, 100f, 10f, titleRect.yMax);
		}

		private void SetBoard(CustomBingoBoard customBoard)
		{
			BingoInitData.Instance.customBoard = customBoard;
		}
	}
}