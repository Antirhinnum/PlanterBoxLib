using Terraria;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Prevents ModPlanterBoxes from being hammered.
		/// </summary>
		private bool On_WorldGen_CanPoundTile(On.Terraria.WorldGen.orig_CanPoundTile orig, int x, int y)
		{
			if (IsPlanterBox(Main.tile[x, y].TileType))
			{
				return false;
			}

			return orig(x, y);
		}
	}
}