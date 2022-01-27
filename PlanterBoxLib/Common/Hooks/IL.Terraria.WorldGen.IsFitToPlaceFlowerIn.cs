using MonoMod.Cil;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Makes ModPlanterBoxes valid tiles to place flowers into.
		/// </summary>
		private void WorldGen_IsFitToPlaceFlowerIn(ILContext il)
		{
			///	Match:
			///		... && tile.type != 380 && ...
			///	Change to:
			///		... && !IsPlanterBox(tile.type) && ...

			DoBasicReplacement(il, expectedValue: true);
		}
	}
}