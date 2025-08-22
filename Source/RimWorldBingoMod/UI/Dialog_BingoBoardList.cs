using RimWorld;
using RimWorldBingoMod.Files;
using System;
using System.IO;
using Verse;

namespace RimWorldBingoMod.UI
{
	public abstract class Dialog_BingoBoardList : Dialog_FileList
	{
		protected override void ReloadFiles()
		{
			files.Clear();
			foreach (FileInfo customBoardFile in GenFilePaths_Bingo.AllCustomBingoBoardsFiles)
			{
				try
				{
					SaveFileInfo saveFileInfo = new SaveFileInfo(customBoardFile);
					saveFileInfo.LoadData();
					files.Add(saveFileInfo);
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + customBoardFile.Name + ": " + ex.ToString());
				}
			}
		}
	}
}