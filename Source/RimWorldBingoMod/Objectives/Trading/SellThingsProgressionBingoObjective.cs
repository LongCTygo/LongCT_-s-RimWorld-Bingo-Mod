using RimWorld;
using RimWorldBingoMod.Broadcasters;
using RimWorldBingoMod.Patches;
using Verse;

namespace RimWorldBingoMod.Objectives.Trading
{
	public class SellThingsProgressionBingoObjective : ProgressionBingoObjective
	{
		public override TaggedString Title => def.title.Translate(goal);

		public override TaggedString Details => def.details.Translate(goal);

		protected override void RegisterToBroadcaster()
		{
			TradeDealMadeBroadcaster.OnTrade += OnTrade;
		}

		protected override void UnregisterFromBroadcaster()
		{
			TradeDealMadeBroadcaster.OnTrade -= OnTrade;
		}

		private void OnTrade(TradeDealResult result)
		{
			if (TradeSession.giftMode)
			{
				return;
			}
			//Ignore trading for honor
			if (TradeSession.trader == null || TradeSession.trader.TradeCurrency == TradeCurrency.Favor)
			{
				return;
			}
			//Prevents overselling
			if (!result.TraderHasEnoughSilver)
			{
				Achieve(result.TraderSilver);
				return;
			}
			float totalSilverValueGot = 0;
			int count = 0;
			foreach (var item in result.ItemsSold)
			{
				if (!item.isCurrency)
				{
					float price = item.price;
					totalSilverValueGot += price;
					count++;
				}
			}
			//Log.Message($"Player sold {totalSilverValueGot} silver worth from {count} sold items).");
			Achieve((int)totalSilverValueGot);
		}
	}
}