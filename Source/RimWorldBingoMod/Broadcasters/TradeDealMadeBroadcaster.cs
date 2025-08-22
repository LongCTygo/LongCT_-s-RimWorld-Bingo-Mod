using RimWorldBingoMod.Patches;
using System;

namespace RimWorldBingoMod.Broadcasters
{
	public static class TradeDealMadeBroadcaster
	{
		public static event Action<TradeDealResult> OnTrade;

		internal static void Notify(TradeDealResult tradeDealResult)
		{
			OnTrade?.Invoke(tradeDealResult);
		}
	}
}