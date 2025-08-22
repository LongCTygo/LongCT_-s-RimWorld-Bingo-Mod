using RimWorld;
using RimWorldBingoMod.Bingo;
using RimWorldBingoMod.Objectives;
using RimWorldBingoMod.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorldBingoMod.Utils
{
	public static class BingoUIUtils
	{
		public static void DrawBoard(Rect rect, List<BingoObjective> board, float cellSize, float padding, float titleYMax)
		{
			if (board == null) return;

			float boardWidth = (cellSize + padding) * 5 - padding;
			float boardHeight = (cellSize + padding) * 5 - padding;

			float startX = rect.x + (rect.width - boardWidth) / 2f;
			float startY = titleYMax + padding;

			for (int i = 0; i < board.Count; i++)
			{
				int col = i % 5;
				int row = i / 5;
				Rect cellRect = new Rect(
						startX + col * (cellSize + padding),
						startY + row * (cellSize + padding),
						cellSize,
						cellSize
					);
				DrawObjectiveCell(cellRect, board[i]);
			}
		}

		public static void DoBingoSettingsMenu(ref float num2, float width)
		{
			num2 += 40f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "Bingo.UI.BingoSettings".Translate());
			if (Widgets.ButtonText(new Rect(200f, num2, width, 30f), "Edit".Translate() + "..."))
			{
				Find.WindowStack.Add(new Dialog_BingoConfig());
			}
		}

		private static void DrawObjectiveCell(Rect rect, BingoObjective objective)
		{
			Color borderColor = objective?.isCompleted == true ? Color.green : Color.gray;
			Widgets.DrawBoxSolidWithOutline(rect, new Color(0.15f, 0.15f, 0.15f), borderColor);

			if (objective != null)
			{
				var label = objective.Title;
				if (objective is ProgressionBingoObjective pbo)
				{
					label += "\n" + pbo.GetProgressionString();
				}
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect, label);

				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, objective.Details);
				}
			}
		}

		public static void DrawBingoCenterList(Listing_Standard listing_Standard)
		{
			IEnumerable<BingoCenterMode> modes = Enum.GetValues(typeof(BingoCenterMode)).Cast<BingoCenterMode>();
			foreach (BingoCenterMode mode in modes)
			{
				var label = "";
				switch (mode)
				{
					case BingoCenterMode.FreeSpace:
						label = "Bingo.UI.CenterModeFreeSpace";
						break;

					case BingoCenterMode.HarderThanOthers:
						listing_Standard.Gap(10f);
						label = "Bingo.UI.CenterModeHarder";
						break;

					case BingoCenterMode.SameAsOthers:
						listing_Standard.Gap(10f);
						label = "Bingo.UI.CenterModeSame";
						break;

					case BingoCenterMode.EasierThanOthers:
						listing_Standard.Gap(10f);
						label = "Bingo.UI.CenterModeEasier";
						break;
				}
				if (listing_Standard.RadioButton(label.Translate(), BingoInitData.Instance.centerMode == mode))
				{
					BingoInitData.Instance.centerMode = mode;
				}
			}
		}
	}
}