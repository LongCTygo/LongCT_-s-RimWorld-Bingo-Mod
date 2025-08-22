using System;
using Verse;

namespace RimWorldBingoMod.Broadcasters
{
	public static class SurgeryDealDamageBroadcaster
	{
		public static event Action<Pawn, int> DamageDealt;

		public static void Notify(Pawn pawn, int damage)
		{
			DamageDealt?.Invoke(pawn, damage);
		}
	}
}