using System;
using System.Collections.Generic;
using Verse;

namespace RimWorldBingoMod.Objectives
{
	public class BingoObjectiveDef : Def
	{
		public string title;
		public string details;
		public Type eventHandlerClass;
		public int difficulty;
		public BingoCategoryDef categoryKey;
		public double weight = 1;
		public bool randomlySelectable = true;
		public bool randomlySelectableWhenPrestige = true;

		public List<ObjectiveCandidate<string>> candidates;
		public IntRange goalRange;
	}
}