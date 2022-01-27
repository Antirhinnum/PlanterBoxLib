using MonoMod.Cil;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Makes Skeletron Prime's bombs explode on contact with ModPlanterBoxes.
		/// </summary>
		private void Projectile_AI_016(ILContext il)
		{
			///	Match:
			///		if (tile != null && tile.active() && (TileID.Sets.Platforms[tile.type] || tile.type == 380))
			///	Change to:
			///		if (tile != null && tile.active() && (TileID.Sets.Platforms[tile.type] || IsPlanterBox(tile.type)))

			DoBasicReplacement(il, expectedValue: false);
		}
	}
}