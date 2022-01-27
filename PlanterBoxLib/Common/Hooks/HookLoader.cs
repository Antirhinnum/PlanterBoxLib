using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader : ILoadable
	{
		private const string ILFailMessage = "PlanterBoxLib could not patch {0}.";
		private static Mod _mod;

		public void Load(Mod mod)
		{
			_mod = mod;
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

		private static bool IsPlanterBox(int tileType)
		{
			return tileType == TileID.PlanterBox || TileLoader.GetTile(tileType) is ModPlanterBox;
		}

		private static void DoBasicReplacement(ILContext il, bool expectedValue)
		{
			DoBasicReplacement(new ILCursor(il), expectedValue);
		}

		/// <summary>
		/// Replaces
		/// </summary>
		/// <param name="c"></param>
		/// <param name="expectedValue">Set to true if the IL uses beq.s and false if the IL uses bne.un.s.</param>
		private static void DoBasicReplacement(ILCursor c, bool expectedValue)
		{
			ILLabel label = null;
			if (!c.TryGotoNext(MoveType.Before,
					i => i.MatchLdfld<Tile>(nameof(Tile.type)),
					i => i.MatchLdcI4(TileID.PlanterBox),
					i => expectedValue ? i.MatchBeq(out label) : i.MatchBneUn(out label)
				))
			{
				LogHookFailed(c.Context);
				return;
			}

			c.Index += 1;
			c.RemoveRange(2);
			c.EmitDelegate(IsPlanterBox);
			c.Emit(expectedValue ? OpCodes.Brtrue_S : OpCodes.Brfalse_S, label);
		}

		private static void LogHookFailed(ILContext failingIL)
		{
			_mod.Logger.ErrorFormat(ILFailMessage, failingIL.Method.FullName);
			throw new Exception(string.Format(ILFailMessage, failingIL.Method.FullName));
		}
	}
}