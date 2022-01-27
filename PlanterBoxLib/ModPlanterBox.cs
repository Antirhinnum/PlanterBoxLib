using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlanterBoxLib
{
	public abstract class ModPlanterBox : ModTile
	{
	}

	public abstract class ModPlanterBox<T> : ModPlanterBox where T : ModItem
	{
		public override sealed void SetStaticDefaults()
		{
			SafeSetStaticDefaults();

			Main.tileFrameImportant[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			TileID.Sets.IgnoresNearbyHalfbricksWhenDrawn[Type] = true;

			ItemDrop = ModContent.ItemType<T>();
		}

		public virtual void SafeSetStaticDefaults()
		{
		}
	}
}