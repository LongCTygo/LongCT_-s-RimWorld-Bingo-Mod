using System;
using Verse;

namespace RimWorldBingoMod.Extensions
{
	public static class TimeDisplayExtension
	{
		// Method from source code, in statistics tab
		public static string ToRealTimeStringDisplay(this int playTime)
		{
			var timeSpan = new TimeSpan(0, 0, playTime);
			return $"{timeSpan.Days}{"LetterDay".Translate()} " +
				$"{timeSpan.Hours}{"LetterHour".Translate()} " +
				$"{timeSpan.Minutes}{"LetterMinute".Translate()} " +
				$"{timeSpan.Seconds}{"LetterSecond".Translate()}";
		}
	}
}