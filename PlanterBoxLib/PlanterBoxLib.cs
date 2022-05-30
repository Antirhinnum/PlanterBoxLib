using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlanterBoxLib
{
	internal enum CallType
	{
		IsPlanterBoxCheck
	}

	public class PlanterBoxLib : Mod
	{
		public override object Call(params object[] args)
		{
			try
			{
				string callType = (string)args[0];
				switch (callType)
				{
					case nameof(CallType.IsPlanterBoxCheck):
						int tileType = Convert.ToInt32(args[1]);
						return IsPlanterBox(tileType);
				}
			}
			catch (Exception e)
			{
				Logger.Error("Call error: " + e.Message + e.StackTrace);
			}

			return null;
		}

		internal static bool IsPlanterBox(int tileType)
		{
			return tileType == TileID.PlanterBox || TileLoader.GetTile(tileType) is ModPlanterBox;
		}
	}
}