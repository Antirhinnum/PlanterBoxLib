using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ID;

namespace PlanterBoxLib.Common.Hooks
{
	internal partial class HookLoader
	{
		/// <summary>
		/// Properly frames ModPlanterBoxes.
		/// </summary>
		private void WorldGen_TileFrame(ILContext il)
		{
			ILCursor c = new(il);

			///	Match:
			///		if (num == 380)
			///	Change to:
			///		if (IsPlanterBox(380))

			ILLabel label = null;
			if (!c.TryGotoNext(MoveType.Before,
					i => i.MatchLdloc(2),
					i => i.MatchLdcI4(TileID.PlanterBox),
					i => i.MatchBneUn(out label)
				))
			{
				LogHookFailed(il);
				return;
			}

			c.Index += 1;
			c.RemoveRange(2);
			c.EmitDelegate(IsPlanterBox);
			c.Emit(OpCodes.Brfalse_S, label);
		}
	}
}