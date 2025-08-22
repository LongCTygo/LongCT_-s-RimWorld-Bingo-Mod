using RimWorldBingoMod.Bingo;
using RimWorldBingoMod.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorldBingoMod.Utils
{
	public static class BingoGeneratorUtils
	{
		public static void PatchBoard(ref List<BingoObjective> board,
			BingoBoardDifficulty difficulty = BingoBoardDifficulty.Medium,
			BingoCenterMode centerMode = BingoCenterMode.HarderThanOthers,
			bool isPrestige = false)
		{
			var frequency = new Dictionary<BingoCategoryDef, int>();
			var excludedCategory = DefDatabase<BingoCategoryDef>.AllDefs
				.Where((cat) => DefDatabase<BingoObjectiveDef>.AllDefs.Where((o) => o.categoryKey == cat).Count() == 0)
				.ToList();

			foreach (var item in board)
			{
				if (item.def.categoryKey == null)
				{
					continue;
				}
				if (frequency.TryGetValue(item.def.categoryKey, out int freq))
				{
					frequency[item.def.categoryKey] = freq + 1;
				}
				else
				{
					frequency[item.def.categoryKey] = 1;
				}
				if (frequency[item.def.categoryKey] >= item.def.categoryKey.limit)
				{
					excludedCategory.Add(item.def.categoryKey);
				}
			}

			for (int i = board.Count; i < 25; i++)
			{
				bool isCenter = i == 12;
				bool flag = isCenter && centerMode == BingoCenterMode.HarderThanOthers;
				//Base is harder than others
				var minDifficulty = GetMinDifficulty(difficulty, flag);
				var maxDifficulty = GetMaxDifficulty(difficulty, flag);

				if (isCenter && centerMode == BingoCenterMode.FreeSpace)
				{
					var freeSpace = CreateObjective(DefDatabase<BingoObjectiveDef>.GetNamedSilentFail("FreeSpaceObjective"));
					board.Add(freeSpace);
					continue;
				}
				if (isCenter && centerMode == BingoCenterMode.EasierThanOthers)
				{
					minDifficulty = Math.Max(1, minDifficulty - 1);
					maxDifficulty = Math.Max(1, maxDifficulty - 1);
				}
				var selector = DefDatabase<BingoObjectiveDef>.AllDefs
					.Where(def => def.randomlySelectable &&
								  !excludedCategory.Contains(def.categoryKey) &&
								  def.difficulty >= minDifficulty &&
								  def.difficulty <= maxDifficulty);
				if (isPrestige)
				{
					selector = selector.Where((def) => def.randomlySelectableWhenPrestige);
				}
				var allValidDefs = selector.ToList();

				bool generated = false;
				while (allValidDefs.Any())
				{
					var selectedDef = RandomUtils.RandomWeighted(allValidDefs, def => def.weight);
					BingoObjective objective = null;

					for (int j = 0; j < 50; j++)
					{
						objective = CreateObjective(selectedDef);
						if (board.Any(o => objective.IsObjectiveSimilar(o)))
						{
							continue;
						}
						generated = true;
						board.Add(objective);
						if (selectedDef.categoryKey != null)
						{
							if (!frequency.TryGetValue(selectedDef.categoryKey, out var freq))
								freq = 0;

							freq++;
							frequency[selectedDef.categoryKey] = freq;

							if (freq >= selectedDef.categoryKey.limit)
								excludedCategory.Add(selectedDef.categoryKey);
						}
						break;
					}
					if (generated)
						break;
					allValidDefs.Remove(selectedDef);
				}

				if (!generated)
				{
					Log.Error("[BingoMod] Failed to generate a valid objective.");
					var nullObjective = CreateObjective(DefDatabase<BingoObjectiveDef>.GetNamedSilentFail("NullObjective"));
					board.Add(nullObjective);
				}
			}
		}

		public static List<BingoObjective> GenerateNewBoard(BingoBoardDifficulty difficulty = BingoBoardDifficulty.Medium,
			BingoCenterMode centerMode = BingoCenterMode.HarderThanOthers,
			bool isPrestige = false)
		{
			var board = new List<BingoObjective>();
			PatchBoard(ref board, difficulty, centerMode, isPrestige);
			return board;
		}

		public static BingoObjective CreateObjective(BingoObjectiveDef def)
		{
			try
			{
				var obj = (BingoObjective)Activator.CreateInstance(def.eventHandlerClass);
				obj.TryInitialize(def);
				return obj;
			}
			catch (Exception ex)
			{
				Log.Error($"[BingoMod] Failed to create objective {def?.defName}: {ex}");
				return null;
			}
		}

		private static int GetMaxDifficulty(BingoBoardDifficulty difficulty, bool isCenter = false)
		{
			switch (difficulty)
			{
				case BingoBoardDifficulty.Easy:
					return isCenter ? 4 : 3;

				case BingoBoardDifficulty.Medium:
					return isCenter ? 5 : 4;

				case BingoBoardDifficulty.Hard:
					return 5;
			}
			return 5;
		}

		private static int GetMinDifficulty(BingoBoardDifficulty difficulty, bool isCenter = false)
		{
			switch (difficulty)
			{
				case BingoBoardDifficulty.Easy:
					return isCenter ? 3 : 1;

				case BingoBoardDifficulty.Medium:
					return isCenter ? 4 : 1;

				case BingoBoardDifficulty.Hard:
					return isCenter ? 5 : 2;
			}
			return 1;
		}
	}
}