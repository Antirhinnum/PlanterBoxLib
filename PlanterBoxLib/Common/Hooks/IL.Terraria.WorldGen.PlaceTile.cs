using MonoMod.Cil;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Allows plants to be placed into ModPlanterBoxes and allowe torches to be placed onto ModPlanterBoxes.
		/// </summary>
		private void WorldGen_PlaceTile(ILContext il)
		{
			ILCursor c = new(il);

			#region
			///	Match:
			///		else if (Main.tile[i, j + 1].type == 78 || Main.tile[i, j + 1].type == 380 || Main.tile[i, j + 1].type == 579)
			///	Change to:
			///		else if (Main.tile[i, j + 1].type == 78 || IsPlanterBox(Main.tile[i, j + 1].type) || Main.tile[i, j + 1].type == 579)

			DoBasicReplacement(c, expectedValue: true);
			#endregion

			#region
			///	Match:
			///		... tile6.type == 380 && tile6.slope() == 0 ...
			///	Change to:
			///		... IsPlanterBox(tile6.type) && tile6.slope() == 0 ...

			DoBasicReplacement(c, expectedValue: false);
			#endregion

			// The code below patches a segment of PlaceTile() that determines the style of Planter box to place. It is not patched because tModLoader handles this.

			//#region
			/////	V_18: An unnamed variable used in a switch case. Local ID: 18
			/////	Match:
			/////		switch (num)
			/////		...
			/////			case 380:
			/////				tile.frameY = (short)(18 * style);
			/////	Change to:
			/////		switch (num)
			/////		...
			/////			case IsPlanterBox(num): // Yes, I know this isn't valid C#. It works on the IL level, and that's all I care about.
			/////				tile.frameY = (short)(18 * style);

			//ILLabel label = null;
			//if (!c.TryGotoNext(MoveType.Before,
			//		i => i.MatchLdloc(18),
			//		i => i.MatchLdcI4(TileID.PlanterBox),
			//		i => i.MatchBeq(out label)
			//	))
			//{
			//	LogHookFailed(il);
			//	return;
			//}

			//c.Index += 1;
			//c.RemoveRange(2);
			//c.EmitDelegate(IsPlanterBox);
			//c.Emit(OpCodes.Brtrue_S, label);
			//#endregion
		}
	}
}