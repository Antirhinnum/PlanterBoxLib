using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Make smart cursor properly place ModPlanterBoxes.
		/// </summary>
		private void SmartCursorHelper_Step_PlanterBox(ILContext il)
		{
			ILCursor c = new(il);

			Type _SmartCursorHelper = typeof(SmartCursorHelper);
			Type[] nestedTypes = _SmartCursorHelper.GetNestedTypes(BindingFlags.NonPublic);
			Type _SmartCursorUsageInfo = nestedTypes.FirstOrDefault((Type t) => t.Name == "SmartCursorUsageInfo");

			#region
			///	Match:
			///		if (providedInfo.item.createTile != 380 || focusedX != -1 || focusedY != -1)
			///			return;
			///	Change to:
			///		if (!IsPlanterBox(providedInfo.item.createTile) || focusedX != -1 || focusedY != -1)
			///			return;

			ILLabel label = null;
			if (!c.TryGotoNext(MoveType.Before,
					i => i.MatchLdarg(0),
					i => i.MatchLdfld(_SmartCursorUsageInfo.GetField("item")),
					i => i.MatchLdfld<Item>(nameof(Item.createTile)),
					i => i.MatchLdcI4(TileID.PlanterBox),
					i => i.MatchBneUn(out label)
				))
			{
				LogHookFailed(il);
				return;
			}

			c.Index += 3;                       // Move onto TileID.PlanterBox (380)
			c.RemoveRange(2);                   // Remove that instruction and the break.
			c.EmitDelegate(IsPlanterBox);       // Check if the loaded createTile value corresponds to a PLanter Box.
			c.Emit(OpCodes.Brfalse_S, label);   // Return if it doesn't.
			#endregion

			#region
			///	Match:
			///		if (tile.active() && tile.type == 380)
			///	Change to:
			///		if (tile.active() && IsPlanterBox(tile.type))

			if (!c.TryGotoNext(MoveType.Before,
					i => i.MatchLdsflda<Main>(nameof(Main.tile)),
					i => i.MatchLdarg(0),
					i => i.MatchLdfld(_SmartCursorUsageInfo.GetField("screenTargetX")),
					i => i.MatchLdarg(0),
					i => i.MatchLdfld(_SmartCursorUsageInfo.GetField("screenTargetY")),
					i => i.MatchCall(out _),
					i => i.MatchStloc(1),
					i => i.MatchLdloca(1),
					i => i.MatchCall(_Tile_get_type),
					i => i.MatchLdindU2(),
					i => i.MatchLdcI4(TileID.PlanterBox),
					i => i.MatchBneUn(out label)
				))
			{
				LogHookFailed(il);
				return;
			}

			c.Index += 10;
			c.RemoveRange(2);
			c.EmitDelegate(IsPlanterBox);
			c.Emit(OpCodes.Brfalse, label);
			#endregion

			#region
			///	tile: Local ID 4
			/// Match:
			///		if (tile.active() && tile.type == 380)
			///	Change to:
			///		if (tile.active() && IsPlanterBox(tile.type))

			if (!c.TryGotoNext(MoveType.Before,
					i => i.MatchLdloca(4),
					i => i.MatchCall(_Tile_get_type),
					i => i.MatchLdindU2(),
					i => i.MatchLdcI4(TileID.PlanterBox),
					i => i.MatchBneUn(out label)
				))
			{
				LogHookFailed(il);
				return;
			}

			c.Index += 3;
			c.RemoveRange(2);
			c.EmitDelegate(IsPlanterBox);
			c.Emit(OpCodes.Brfalse, label);
			#endregion
		}
	}
}