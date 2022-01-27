using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlanterBoxLib
{
	public abstract class ModPlanterBoxItem<T> : ModItem where T : ModPlanterBox
	{
		public override sealed void SetStaticDefaults()
		{
			SafeSetStaticDefaults();

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override sealed void SetDefaults()
		{
			SafeSetDefaults();

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 20;
			Item.value = Terraria.Item.buyPrice(0, 0, 1);

			Item.createTile = ModContent.TileType<T>();
		}

		public virtual void SafeSetStaticDefaults()
		{
		}

		public virtual void SafeSetDefaults()
		{
		}
	}
}