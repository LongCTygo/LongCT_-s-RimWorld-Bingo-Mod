using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse;

namespace RimWorldBingoMod.Files
{
	public static class GenFilePaths_Bingo
	{
		private static string SaveDataFolderPath => GenFilePaths.SaveDataFolderPath;
		private static string BingoBoardFileExtension => ".bgb";
		public static string BingoBoardsFolderPath => FolderUnderSaveData("BingoBoards");

		public static IEnumerable<FileInfo> AllCustomBingoBoardsFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(BingoBoardsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
					   where f.Extension == BingoBoardFileExtension
					   orderby f.LastWriteTime descending
					   select f;
			}
		}

		//Borrowed directly from decompiled codes, sorry Tynan pls don't sue me
		private static string FolderUnderSaveData(string folderName)
		{
			string text = Path.Combine(SaveDataFolderPath, folderName);
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			return text;
		}

		internal static string AbsPathForBingoBoard(string fileName)
		{
			return Path.Combine(BingoBoardsFolderPath, fileName + BingoBoardFileExtension);
		}
	}
}