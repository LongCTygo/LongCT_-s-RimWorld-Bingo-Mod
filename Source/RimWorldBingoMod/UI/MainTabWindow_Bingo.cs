using RimWorld;
using RimWorldBingoMod.Bingo;
using RimWorldBingoMod.Utils;
using UnityEngine;
using Verse;

namespace RimWorldBingoMod.UI
{
	[StaticConstructorOnStartup]
	public class MainTabWindow_Bingo : MainTabWindow
	{
		private const float CellSize = 100f;
		private const float Padding = 10f;
		private const float TitleHeight = 80f;
		private static readonly Texture2D BingoTitleTexture = ContentFinder<Texture2D>.Get("BingoIcon");

		public override Vector2 RequestedTabSize
		{
			get
			{
				float width = (CellSize + Padding) * 5 + Padding + 50f;
				float height = TitleHeight + (CellSize + Padding) * 5 + Padding * 2 + 75f;
				return new Vector2(width, height);
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			inRect = inRect.ContractedBy(Padding);

			if (BingoBoardComponent.Instance == null || !BingoBoardComponent.Instance.IsValid)
			{
				Widgets.Label(inRect, "No active bingo board.");
				return;
			}

			var board = BingoBoardComponent.Instance.board;
			if (board == null)
			{
				Widgets.Label(inRect, "Board not initialized.");
				return;
			}

			// Draw "BINGO" texture centered at top
			float titleWidth = 535f;
			Rect titleRect = new Rect(
				inRect.x + (inRect.width - titleWidth) / 2f,
				inRect.y + 25f,
				titleWidth,
				TitleHeight
			);
			GUI.DrawTexture(titleRect, BingoTitleTexture, ScaleMode.ScaleToFit, true);

			// --- Button at top-right (aligned with title height) ---
			float buttonWidth = 100f;
			float buttonHeight = 25f;
			Rect buttonRect = new Rect(
				inRect.xMax - buttonWidth,  // right edge of window
				inRect.y, // vertically centered with title
				buttonWidth,
				buttonHeight
			);

			if (BingoBoardComponent.Instance.canPrestige && Widgets.ButtonText(buttonRect, "Bingo.UI.Prestige".Translate()))
			{
				Find.WindowStack.Add(new Dialog_MessageBox("Bingo.UI.PrestigeConfirmationMessage".Translate(),
					"Confirm".Translate(),
					delegate
					{
						BingoBoardComponent.Instance.Prestige();
						Messages.Message("Bingo.UI.PrestigeDone".Translate(), MessageTypeDefOf.NeutralEvent);
					},
					"Cancel".Translate()
				));
			}

			// Draw the board below the title
			BingoUIUtils.DrawBoard(inRect, board, CellSize, Padding, titleRect.yMax + 10f);
		}
	}
}