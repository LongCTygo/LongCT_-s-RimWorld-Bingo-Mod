using RimWorldBingoMod.Bingo;
using System;
using System.IO;
using Verse;

namespace RimWorldBingoMod.Files
{
	public static class GameDataSaveLoader_Bingo
	{
		public static bool TryLoadBingoBoard(string absPath, out CustomBingoBoard customBoard)
		{
			customBoard = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.None, logVersionConflictWarning: true);
					Scribe_Deep.Look(ref customBoard, "customBoard");
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				customBoard.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading bingo board: " + ex.ToString());
				customBoard = null;
				Scribe.ForceStop();
			}
			return customBoard != null;
		}

		public static void SaveBingoBoard(CustomBingoBoard customBoard, string absFilePath)
		{
			try
			{
				customBoard.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedBingoBoard", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look(ref customBoard, "customBoard");
				});
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving world: " + ex.ToString());
			}
		}
	}
}