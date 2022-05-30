using System;
using System.Linq;
using RFVehicleLockLimiter.Models;
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
                    StringComparison.OrdinalIgnoreCase)).ToList();
            if (permissions.Count == 0)
                return Plugin.Conf.DefaultVehicleLockLimit;

            uint bestLockLimit = 0;
            foreach (var perm in permissions)
            {
                var pocketSplit = perm.Split('.');
                if (pocketSplit.Length != 2)
                {
                    Logger.LogError($"[{Plugin.Inst.Name}] Invalid permission format: {perm}");
                    Logger.LogError($"[{Plugin.Inst.Name}] Correct format: {Permissions.Lock}'amount'");
                    continue;
                }

                byte.TryParse(pocketSplit.ElementAtOrDefault(1), out var limit);
                if (limit > bestLockLimit)
                    bestLockLimit = limit;
            }

            return bestLockLimit;
        }

        internal static int GetVehicleLockedCount(ulong steamId)
        {
            return VehicleManager.vehicles.Count(
                vehicle => vehicle.lockedOwner.m_SteamID == steamId && vehicle.isLocked && !Plugin.Conf.IgnoredIDs.Contains(new Vehicle {Id = vehicle.asset.id}));
        }

        internal static int CountRFGarage(ulong steamId)
        {
            return RFGarage.DatabaseManagers.GarageManager.Count(steamId);
        }
    }
}