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
			if (Main.tile[x, y] == null)
			{
				Main.tile[x, y] = new Tile();
			}

			if (Main.tile[x, y - 1] == null)
			{
				Main.tile[x, y - 1] = new Tile();
			}

			if (Main.tile[x, y + 1] == null)
			{
				Main.tile[x, y + 1] = new Tile();
			}

			return IsPlanterBox(Main.tile[x, y].type) ? false : orig(x, y);
		}
	}
}