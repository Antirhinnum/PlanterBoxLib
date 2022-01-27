using PlanterBoxLib;
using Terraria.ModLoader;

namespace PlanterBoxLibTest
{
	/// <summary>
	/// This is the simplest possible example of the library. Just inheriting from the provided classes will handle pretty much everything to make the item and tile function.
	/// Be aware that ModPlanterBoxItem and ModPlanterBox inherit from ModItem and ModTile respectively, so any normal extra functionality can be done through those classes' methods.
	/// </summary>
	public class PlanterBoxLibTest : Mod
	{
	}
	public class ExamplePlanterBoxItem : ModPlanterBoxItem<ExamplePlanterBox>
	{
	}

	public class ExamplePlanterBox : ModPlanterBox<ExamplePlanterBoxItem>
	{
	}
}