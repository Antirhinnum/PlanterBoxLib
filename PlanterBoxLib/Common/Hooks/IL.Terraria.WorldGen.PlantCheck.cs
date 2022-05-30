using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ID;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Allows herbs to grow in ModPlanterBoxes.
		/// </summary>
		private void WorldGen_PlantCheck(ILContext il)
		{
			ILCursor c = new(il);

			///	Match:
			///		... || num == 380 || ...
			///	Change to:
			///		... || IsPlanterBox(num) || ...

			ILLabel label = null;
			for (int i = 0; i < 2; i++)
			{
				if (!c.TryGotoNext(MoveType.Before,
						i => i.MatchLdloc(0),
						i => i.MatchLdcI4(TileID.PlanterBox),
						i => i.MatchBeq(out label)
					))
				{
					LogHookFailed(il);
					return;
				}

				c.Index += 1;
				c.RemoveRange(2);
				c.EmitDelegate(PlanterBoxLib.IsPlanterBox);
				c.Emit(OpCodes.Brtrue_S, label);
			}
		}
	}
}