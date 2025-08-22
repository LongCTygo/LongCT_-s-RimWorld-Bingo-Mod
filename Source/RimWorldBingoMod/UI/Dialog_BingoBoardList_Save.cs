using RimWorld;
using RimWorldBingoMod.Bingo;
using RimWorldBingoMod.Files;
using System;
using System.IO;
using System.Linq;
using Verse;

namespace RimWorldBingoMod.UI
{
	public class Dialog_BingoBoardList_Save : Dialog_BingoBoardList
	{
		private CustomBingoBoard savingCustomBoard;
		private Action onClosed;

		protected override bool ShouldDoTypeInField => true;

		public string DefaultTypingName
		{
			get
			{
				string text = "";
				int num = 1;
				do
				{
					text = "BingoBoard" + num;
					num++;
				} while (NameExist(text));
				return text;
			}
		}

		private bool NameExist(string fileName)
		{
			foreach (string item in GenFilePaths_Bingo.AllCustomBingoBoardsFiles.Select((FileInfo f) => Path.GetFileNameWithoutExtension(f.Name)))
			{
				if (item == fileName)
				{
					return true;
				}
			}
			return false;
		}

		public Dialog_BingoBoardList_Save(CustomBingoBoard customBoard, Action onClosed = null)
		{
			interactButLabel = "OverwriteButton".Translate();
			savingCustomBoard = customBoard;
			this.onClosed = onClosed;
			//typing name
			typingName = string.IsNullOrEmpty(customBoard.fileName) ? DefaultTypingName : customBoard.fileName;
		}

		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths_Bingo.AbsPathForBingoBoard(fileName);
			LongEventHandler.QueueLongEvent(delegate
			{
				GameDataSaveLoader_Bingo.SaveBingoBoard(savingCustomBoard, absPath);
			}, "SavingLongEvent", doAsynchronously: false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, historical: false);
			Close();
		}

		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			onClosed?.Invoke();
		}
	}
}