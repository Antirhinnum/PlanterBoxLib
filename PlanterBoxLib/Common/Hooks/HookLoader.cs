using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader : ILoadable
	{
		private const string ILFailMessage = "PlanterBoxLib could not patch {0}.";
		private static Mod _mod;
		private static MethodInfo _Tile_get_type;

		public void Load(Mod mod)
		{
			_mod = mod;
			_Tile_get_type = typeof(Tile).GetProperty("type", BindingFlags.Instance | BindingFlags.NonPublic).GetMethod;

			IL.Terraria.GameContent.SmartCursorHelper.Step_AlchemySeeds += SmartCursorHelper_Step_AlchemySeeds;
			IL.Terraria.GameContent.SmartCursorHelper.Step_PlanterBox += SmartCursorHelper_Step_PlanterBox;
			IL.Terraria.Player.PlaceThing_Tiles_BlockPlacementForAssortedThings += Player_PlaceThing_Tiles_BlockPlacementForAssortedThings;
			IL.Terraria.Projectile.AI_016 += Projectile_AI_016;
			IL.Terraria.WorldGen.PlaceAlch += WorldGen_PlaceAlch;
			IL.Terraria.WorldGen.CheckAlch += WorldGen_CheckAlch;
			IL.Terraria.WorldGen.IsFitToPlaceFlowerIn += WorldGen_IsFitToPlaceFlowerIn;
			IL.Terraria.WorldGen.PlaceTile += WorldGen_PlaceTile;
			IL.Terraria.WorldGen.UpdateWorld_OvergroundTile += WorldGen_UpdateWorld_OvergroundTile;
			IL.Terraria.WorldGen.PlantCheck += WorldGen_PlantCheck;
			IL.Terraria.WorldGen.TileFrame += WorldGen_TileFrame;

			On.Terraria.WorldGen.CanPoundTile += On_WorldGen_CanPoundTile;
			On.Terraria.WorldGen.CanCutTile += On_WorldGen_CanCutTile;
		}

		public void Unload()
		{
			_mod = null;
			_Tile_get_type = null;

			IL.Terraria.GameContent.SmartCursorHelper.Step_AlchemySeeds -= SmartCursorHelper_Step_AlchemySeeds;
			IL.Terraria.GameContent.SmartCursorHelper.Step_PlanterBox -= SmartCursorHelper_Step_PlanterBox;
			IL.Terraria.Player.PlaceThing_Tiles_BlockPlacementForAssortedThings -= Player_PlaceThing_Tiles_BlockPlacementForAssortedThings;
			IL.Terraria.Projectile.AI_016 -= Projectile_AI_016;
			IL.Terraria.WorldGen.PlaceAlch -= WorldGen_PlaceAlch;
			IL.Terraria.WorldGen.CheckAlch -= WorldGen_CheckAlch;
			IL.Terraria.WorldGen.IsFitToPlaceFlowerIn -= WorldGen_IsFitToPlaceFlowerIn;
			IL.Terraria.WorldGen.PlaceTile -= WorldGen_PlaceTile;
			IL.Terraria.WorldGen.UpdateWorld_OvergroundTile -= WorldGen_UpdateWorld_OvergroundTile;
			IL.Terraria.WorldGen.PlantCheck -= WorldGen_PlantCheck;
			IL.Terraria.WorldGen.TileFrame -= WorldGen_TileFrame;

			On.Terraria.WorldGen.CanPoundTile -= On_WorldGen_CanPoundTile;
			On.Terraria.WorldGen.CanCutTile -= On_WorldGen_CanCutTile;
		}

		private static void DoBasicReplacement(ILContext il, bool expectedValue)
		{
			DoBasicReplacement(new ILCursor(il), expectedValue);
		}

		/// <summary>
		/// Replaces a basic type check for TileID.PlanerBox with one that uses IsPlanterBox().
		/// </summary>
		/// <param name="expectedValue">Set to true if the IL uses beq.s and false if the IL uses bne.un.s.</param>
		private static void DoBasicReplacement(ILCursor c, bool expectedValue)
		{
			ILLabel label = null;
			if (!c.TryGotoNext(MoveType.Before,
					i => i.MatchCall(_Tile_get_type),
					i => i.MatchLdindU2(),
					i => i.MatchLdcI4(TileID.PlanterBox),
					i => expectedValue ? i.MatchBeq(out label) : i.MatchBneUn(out label)
				))
			{
				LogHookFailed(c.Context);
				return;
			}

			c.Index += 2;
			c.RemoveRange(2);
			c.EmitDelegate(PlanterBoxLib.IsPlanterBox);
			c.Emit(expectedValue ? OpCodes.Brtrue_S : OpCodes.Brfalse_S, label);
		}

		private static void LogHookFailed(ILContext failingIL)
		{
			_mod.Logger.ErrorFormat(ILFailMessage, failingIL.Method.FullName);
			throw new Exception(string.Format(ILFailMessage, failingIL.Method.FullName));
		}
	}
}