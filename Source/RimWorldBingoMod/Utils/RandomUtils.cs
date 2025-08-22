using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorldBingoMod.Utils
{
	public static class RandomUtils
	{
		public static T RandomWeighted<T>(IEnumerable<T> items, Func<T, double> weightSelector)
		{
			if (items == null)
				return default;
			var list = items.ToList();
			if (list.Count == 0)
				return default;
			double totalWeight = list.Sum(weightSelector);
			if (totalWeight <= 0)
				return default;
			float rand = Rand.Range(0f, (float)totalWeight);
			double cumulative = 0;
			foreach (var item in list)
			{
				cumulative += weightSelector(item);
				if (rand <= cumulative)
					return item;
			}
			return list.Last();
		}
	}
}