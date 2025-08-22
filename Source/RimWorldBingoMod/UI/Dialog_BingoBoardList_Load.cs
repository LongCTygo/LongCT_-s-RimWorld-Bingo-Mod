using RimWorldBingoMod.Bingo;
using RimWorldBingoMod.Files;
using System;
using Verse;

namespace RimWorldBingoMod.UI
{
	public class Dialog_BingoBoardList_Load : Dialog_BingoBoardList
	{
		private Action<CustomBingoBoard> listReturner;

		public Dialog_BingoBoardList_Load(Action<CustomBingoBoard> listReturner)
		{
			this.listReturner = listReturner;
			interactButLabel = "LoadGameButton".Translate();
		}

		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths_Bingo.AbsPathForBingoBoard(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.None, delegate
			{
				if (GameDataSaveLoader_Bingo.TryLoadBingoBoard(filePath, out CustomBingoBoard board))
				{
					listReturner(board);
				}
				Close();
			});
		}
	}
}