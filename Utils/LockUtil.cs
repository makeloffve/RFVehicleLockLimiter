using System;
using System.Linq;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace RFVehicleLockLimiter.Utils
{
    public static class LockUtil
    {
        internal static uint GetBestLockLimit(UnturnedPlayer player)
        {
            var permissions = player.GetPermissions().Select(a => a.Name).Where(p =>
                p.ToLower().StartsWith(Permissions.Lock) && !p.Equals(Permissions.Lock,
                    StringComparison.OrdinalIgnoreCase));
            var enumerable = permissions as string[] ?? permissions.ToArray();
            if (enumerable.Length == 0)
                return Plugin.Conf.DefaultVehicleLockLimit;

            uint bestLockLimit = 0;
            foreach (var pocket in enumerable)
            {
                var pocketSplit = pocket.Split('.');
                if (pocketSplit.Length != 2)
                {
                    Logger.LogError($"[{Plugin.Inst.Name}] Error: PermissionPrefix must not contain '.'");
                    Logger.LogError($"[{Plugin.Inst.Name}] Invalid permission format: {pocket}");
                    Logger.LogError($"[{Plugin.Inst.Name}] Correct format: 'permPrefix'.'amount'");
                    continue;
                }

                try
                {
                    byte.TryParse(pocketSplit[1], out var limit);
                    if (limit > bestLockLimit)
                        bestLockLimit = limit;
                }
                catch (Exception ex)
                {
                    bestLockLimit = Plugin.Conf.DefaultVehicleLockLimit;
                    Logger.LogError($"[{Plugin.Inst.Name}] Error: " + ex);
                }
            }
            
            return bestLockLimit;
        }

        internal static int GetVehicleLockedCount(ulong steamId)
        {
            return VehicleManager.vehicles.Count(vehicle => vehicle.lockedOwner.m_SteamID == steamId);
        }
    }
}