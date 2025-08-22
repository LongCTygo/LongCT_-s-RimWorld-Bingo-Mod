using RimWorldBingoMod.Objectives;
using System.Collections.Generic;
using Verse;

namespace RimWorldBingoMod.Bingo
{
	public class CustomBingoBoard : IExposable
	{
		public List<BingoObjective> board = new List<BingoObjective>();
		public string fileName;

		public void ExposeData()
		{
			Scribe_Collections.Look(ref board, "board", LookMode.Deep);
		}
	}
}