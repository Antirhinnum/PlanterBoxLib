using Terraria;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Prevents plants in ModPlanterBoxes from being cut down unintentionally.
		/// </summary>
		private bool On_WorldGen_CanCutTile(On.Terraria.WorldGen.orig_CanCutTile orig, int x, int y, Terraria.Enums.TileCuttingContext context)
		{
			if (PlanterBoxLib.IsPlanterBox(Main.tile[x, y + 1].TileType))
			{
				return false;
			}

			return orig(x, y, context);
		}
	}
}