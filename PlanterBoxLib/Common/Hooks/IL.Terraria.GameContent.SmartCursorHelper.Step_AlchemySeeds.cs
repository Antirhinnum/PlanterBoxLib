using MonoMod.Cil;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Makes herb seeds lock to ModPlanterBoxes if Smart Cursor is enabled.
		/// </summary>
		private void SmartCursorHelper_Step_AlchemySeeds(ILContext il)
		{
			ILCursor c = new(il);

			#region
			///	Match:
			///		... && tile2.type != 380 && ...
			///	Change to:
			///		... && !IsPlanterBox(tile2.type) && ...

			for (int i = 0; i < 7; i++)
			{
				DoBasicReplacement(c, expectedValue: true);
			}
			#endregion
		}
	}
}