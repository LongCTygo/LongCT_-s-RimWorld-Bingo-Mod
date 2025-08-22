using RimWorldBingoMod.Bingo;
using RimWorldBingoMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorldBingoMod.UI
{
	public class Dialog_BingoConfig : Window
	{
		public override Vector2 InitialSize => new Vector2(500f, 600f);

		public Dialog_BingoConfig()
		{
			doCloseButton = true;
			forcePause = true;
			absorbInputAroundWindow = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = 200f;
			listing_Standard.Begin(inRect.AtZero());
			Text.Font = GameFont.Medium;
			listing_Standard.Label("Bingo.UI.BingoMode".Translate());
			Text.Font = GameFont.Small;
			IEnumerable<BingoMode> modes = Enum.GetValues(typeof(BingoMode)).Cast<BingoMode>();
			foreach (BingoMode mode in modes)
			{
				var label = "";
				switch (mode)
				{
					case BingoMode.Disabled:
						label = "Bingo.UI.ModeDisabled";
						break;

					case BingoMode.Random:
						listing_Standard.Gap(10f);
						label = "Bingo.UI.ModeRandom";
						break;

					case BingoMode.Custom:
						listing_Standard.Gap(10f);
						label = "Bingo.UI.ModeCustom";
						break;
				}
				if (listing_Standard.RadioButton(label.Translate(), BingoInitData.Instance.bingoMode == mode))
				{
					BingoInitData.Instance.bingoMode = mode;
				}
			}
			if (BingoInitData.Instance.bingoMode == BingoMode.Custom)
			{
				if (BingoInitData.Instance.customBoard.board.Count != 25)
				{
					BingoGeneratorUtils.PatchBoard(ref BingoInitData.Instance.customBoard.board);
				}
				listing_Standard.Gap(20f);
				if (listing_Standard.ButtonText("Bingo.UI.CustomizeBoard".Translate()))
				{
					Find.WindowStack.Add(new Dialog_BingoBoardCustomize());
				}
			}
			else if (BingoInitData.Instance.bingoMode == BingoMode.Random)
			{
				listing_Standard.Gap(20f);
				Text.Font = GameFont.Medium;
				listing_Standard.Label("Bingo.UI.CenterMode".Translate());
				Text.Font = GameFont.Small;
				BingoUIUtils.DrawBingoCenterList(listing_Standard);
				listing_Standard.Gap(20f);
				Text.Font = GameFont.Medium;
				listing_Standard.Label("Bingo.UI.Difficulty".Translate());
				Text.Font = GameFont.Small;
				IEnumerable<BingoBoardDifficulty> difficulties = Enum.GetValues(typeof(BingoBoardDifficulty)).Cast<BingoBoardDifficulty>();
				foreach (BingoBoardDifficulty difficulty in difficulties)
				{
					var label = "";
					switch (difficulty)
					{
						case BingoBoardDifficulty.Easy:
							label = "Bingo.UI.Difficulty.Easy";
							break;

						case BingoBoardDifficulty.Medium:
							listing_Standard.Gap(10f);
							label = "Bingo.UI.Difficulty.Medium";
							break;

						case BingoBoardDifficulty.Hard:
							listing_Standard.Gap(10f);
							label = "Bingo.UI.Difficulty.Hard";
							break;
					}
					if (listing_Standard.RadioButton(label.Translate(), BingoInitData.Instance.difficulty == difficulty))
					{
						BingoInitData.Instance.difficulty = difficulty;
					}
				}
			}
			listing_Standard.NewColumn();
			Text.Font = GameFont.Small;
			listing_Standard.Label("");
			switch (BingoInitData.Instance.bingoMode)
			{
				case BingoMode.Disabled:
					listing_Standard.Label("Bingo.UI.ModeDisabled.description".Translate());
					break;

				case BingoMode.Random:
					listing_Standard.Label("Bingo.UI.ModeRandom.description".Translate());
					break;

				case BingoMode.Custom:
					listing_Standard.Label("Bingo.UI.ModeCustom.description".Translate());
					break;
			}
			listing_Standard.End();
		}
	}
}