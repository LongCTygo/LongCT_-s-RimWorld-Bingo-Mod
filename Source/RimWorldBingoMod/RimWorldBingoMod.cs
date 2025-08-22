using HarmonyLib;
using System;
using Verse;

namespace RimWorldBingoMod
{
	[StaticConstructorOnStartup]
	public class RimWorldBingoMod : Mod
	{
		private static Harmony _harmony;
		public static RimWorldBingoMod Instance { get; private set; }

		public RimWorldBingoMod(ModContentPack content) : base(content)
		{
			Instance = this;
			_harmony = new Harmony("LongCT.RimWorldBingoMod");
			try
			{
				_harmony.PatchAll();
				Log.Message("[RimWorldBingoMod] Successfully patched all methods");
			}
			catch (Exception ex)
			{
				Log.Error($"[RimWorldBingoMod] Failed to patch methods: {ex}");
			}
		}
	}
}