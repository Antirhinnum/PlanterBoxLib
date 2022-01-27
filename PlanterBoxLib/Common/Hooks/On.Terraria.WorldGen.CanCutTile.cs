using Terraria;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		private bool On_WorldGen_CanCutTile(On.Terraria.WorldGen.orig_CanCutTile orig, int x, int y, Terraria.Enums.TileCuttingContext context)
		{
			if (Main.tile[x, y + 1] is not null && IsPlanterBox(Main.tile[x, y + 1].type))
			{
				return false;
			}

			return orig(x, y, context);
		}
	}
}