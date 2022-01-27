using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlanterBoxLib
{
	/// <summary>
	/// DO NOT INHERIT FROM THIS. This class exists to more easily check if a ModTile is a ModPlanterBox.
	/// </summary>
	public abstract class ModPlanterBox : ModTile
	{
	}

	/// <summary>
	/// A modded Planter Box tile.
	/// </summary>
	/// <typeparam name="T">The ModItem that is used to place this ModPlanterBox.</typeparam>
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