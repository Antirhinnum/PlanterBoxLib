using MonoMod.Cil;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Allows plants to naturally grow on ModPlanterBoxes.
		/// </summary>
		private void WorldGen_UpdateWorld_OvergroundTile(ILContext il)
		{
			///	Match:
			///		else if (Main.tile[i, j].type == 78 || Main.tile[i, j].type == 380 || Main.tile[i, j].type == 579)
			///	Change to:
			///		else if (Main.tile[i, j].type == 78 || IsPlanterBox(Main.tile[i, j].type) || Main.tile[i, j].type == 579)

			DoBasicReplacement(il, expectedValue: true);
		}
	}
}