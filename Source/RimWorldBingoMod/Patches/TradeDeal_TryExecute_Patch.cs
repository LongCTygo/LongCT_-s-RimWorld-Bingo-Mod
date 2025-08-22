using HarmonyLib;
using RimWorld;
using RimWorldBingoMod.Broadcasters;
using System.Collections.Generic;
using Verse;

namespace RimWorldBingoMod.Patches
{
	[HarmonyPatch(typeof(TradeDeal), nameof(TradeDeal.TryExecute))]
	public class TradeDeal_TryExecute_Patch
	{
		public static void Prefix(out TradeDealResult __state, TradeDeal __instance, List<Tradeable> ___tradeables)
		{
			__state = new TradeDealResult()
			{
				TraderHasEnoughSilver = __instance.DoesTraderHaveEnoughSilver(),
				TraderSilver = __instance.CurrencyTradeable?.CountHeldBy(Transactor.Trader) ?? 0
			};
			foreach (Tradeable tradeable in ___tradeables)
			{
				if (tradeable.ActionToDo == TradeAction.PlayerBuys)
				{
					var item = new TradeDealItem(tradeable.AnyThing, tradeable.CurTotalCurrencyCostForSource, tradeable.IsCurrency);
					__state.ItemsBought.Add(item);
				}
				else if (tradeable.ActionToDo == TradeAction.PlayerSells)
				{
					var item = new TradeDealItem(tradeable.AnyThing, tradeable.CurTotalCurrencyCostForDestination, tradeable.IsCurrency);
					__state.ItemsSold.Add(item);
				}
			}
		}

		public static void Postfix(bool __result, TradeDealResult __state)
		{
			if (__result)
			{
				TradeDealMadeBroadcaster.Notify(__state);
			}
		}
	}

	public class TradeDealResult
	{
		public bool TraderHasEnoughSilver { get; set; }
		public int TraderSilver { get; set; }
		public List<TradeDealItem> ItemsSold { get; set; } = new List<TradeDealItem>();
		public List<TradeDealItem> ItemsBought { get; set; } = new List<TradeDealItem>();
	}

	public class TradeDealItem
	{
		public Thing thing;
		public float price;
		public bool isCurrency;

		public TradeDealItem(Thing thing, float price, bool isCurrency)
		{
			this.thing = thing;
			this.price = price;
			this.isCurrency = isCurrency;
		}
	}
}