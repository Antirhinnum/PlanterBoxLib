using MonoMod.Cil;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Prevents the player from breaking seeds planted in ModPlanterBoxes by placing a new seed on top. Probably.
		/// </summary>
		private void Player_PlaceThing_Tiles_BlockPlacementForAssortedThings(ILContext il)
		{
			///	Match:
			///		bool num11 = Main.tile[tileTargetX, tileTargetY + 1].type != 78 && Main.tile[tileTargetX, tileTargetY + 1].type != 380 && Main.tile[tileTargetX, tileTargetY + 1].type != 579;
			///	Change to:
			///		bool num11 = Main.tile[tileTargetX, tileTargetY + 1].type != 78 && !IsPlanterBox(Main.tile[tileTargetX, tileTargetY + 1].type) && Main.tile[tileTargetX, tileTargetY + 1].type != 579;

			DoBasicReplacement(il, expectedValue: true);
		}
	}
}