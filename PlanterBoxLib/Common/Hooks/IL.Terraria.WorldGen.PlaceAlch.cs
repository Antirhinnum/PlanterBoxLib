using MonoMod.Cil;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Allows the placement of Main.tileAlch tiles on ModPlanterBoxes.
		/// </summary>
		private void WorldGen_PlaceAlch(ILContext il)
		{
			ILCursor c = new(il);

			for (int i = 0; i < 7; i++)
			{
				DoBasicReplacement(c, expectedValue: true);
			}
		}
	}
}