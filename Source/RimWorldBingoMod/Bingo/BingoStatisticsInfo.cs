using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorldBingoMod.Bingo
{
	public class BingoStatisticsInfo : IExposable
	{
		public List<int> inGameTimeBingoGets = new List<int>();
		public List<int> realTimeBingoGets = new List<int>();
		private int bingoCount = 0;
		public int strangeSignStage = 0;
		public int rewardsStage = 0;

		private int tickWhenGenerated = 0;
		private int realTimeWhenGenerated = 0;

		public int Bingo
		{
			get
			{
				return bingoCount;
			}
			set
			{
				SetBingoStatistics(value);
			}
		}

		public int CurrentIGT => inGameTimeBingoGets[bingoCount - 1] - tickWhenGenerated;
		public int CurrentRTA => realTimeBingoGets[bingoCount - 1] - realTimeWhenGenerated;
		public List<int> AllIGTs => inGameTimeBingoGets.Select(x => x - tickWhenGenerated).ToList();
		public List<int> AllRTAs => realTimeBingoGets.Select(x => x - realTimeWhenGenerated).ToList();

		public BingoStatisticsInfo()
		{
			tickWhenGenerated = Find.TickManager?.TicksGame ?? 0;
			var realTime = Find.GameInfo?.RealPlayTimeInteracting ?? 0;
			realTimeWhenGenerated = (int)realTime;
			for (int i = 0; i <= 12; i++)
			{
				inGameTimeBingoGets.Add(tickWhenGenerated);
				realTimeBingoGets.Add(realTimeWhenGenerated);
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look(ref inGameTimeBingoGets, "inGameTimeBingoGets", LookMode.Value);
			Scribe_Collections.Look(ref realTimeBingoGets, "realTimeBingoGets", LookMode.Value);
			Scribe_Values.Look(ref bingoCount, "bingoCount");
			Scribe_Values.Look(ref strangeSignStage, "strangeSignStage", 0);
			Scribe_Values.Look(ref rewardsStage, "rewardsStage", 0);
			Scribe_Values.Look(ref tickWhenGenerated, "tickWhenGenerated", 0);
			Scribe_Values.Look(ref realTimeWhenGenerated, "realTimeWhenGenerated", 0);
		}

		private void SetBingoStatistics(int bingoCount)
		{
			if (bingoCount < 0 || bingoCount > 12)
			{
				return;
			}
			if (this.bingoCount >= bingoCount)
			{
				this.bingoCount = bingoCount;
				return;
			}
			for (int i = this.bingoCount; i < bingoCount; i++)
			{
				inGameTimeBingoGets[i] = Find.TickManager.TicksGame;
				realTimeBingoGets[i] = (int)Find.GameInfo.RealPlayTimeInteracting;
			}
			this.bingoCount = bingoCount;
		}
	}
}