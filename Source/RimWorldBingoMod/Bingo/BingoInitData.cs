namespace RimWorldBingoMod.Bingo
{
	public class BingoInitData
	{
		private static BingoInitData instance;

		public static BingoInitData Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BingoInitData();
				}
				return instance;
			}
		}

		public static void ClearInstance()
		{
			instance = null;
		}

		private BingoInitData()
		{
		}

		public BingoMode bingoMode = BingoMode.Random;
		public CustomBingoBoard customBoard = new CustomBingoBoard();
		public BingoBoardDifficulty difficulty = BingoBoardDifficulty.Medium;
		public BingoCenterMode centerMode = BingoCenterMode.HarderThanOthers;
	}
}