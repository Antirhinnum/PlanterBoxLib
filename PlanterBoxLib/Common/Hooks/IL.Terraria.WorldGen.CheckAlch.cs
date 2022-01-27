using MonoMod.Cil;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Makes ModPlanterBoxes a valid tile for herbs to exist on.
		/// </summary>
		private void WorldGen_CheckAlch(ILContext il)
		{
			ILCursor c = new(il);

			///	Match:
			///		... && Main.tile[x, y + 1].type != 380)
			///	Change to:
			///		... && !IsPlanterBox(Main.tile[x, y + 1].type))

			for (int i = 0; i < 7; i++)
			{
				DoBasicReplacement(c, expectedValue: true);
			}
		}
	}
}