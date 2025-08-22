using RimWorld;
using RimWorldBingoMod.Bingo;

namespace RimWorldBingoMod.UI
{
	public class MainButtonWorker_ToggleBingoTab : MainButtonWorker_ToggleTab
	{
		public override bool Disabled => !BingoBoardComponent.Instance.IsValid;
		public override bool Visible => !Disabled;
	}
}