using HarmonyLib;
using RFRocketLibrary.Helpers;
using RFVehicleLockLimiter.Enums;
using RFVehicleLockLimiter.Utils;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace RFVehicleLockLimiter.Patches
{
    [HarmonyPatch]
    internal static class InternalPatch
    {
        [HarmonyPatch(typeof(VehicleManager), "ReceiveVehicleLockRequest")]
        [HarmonyPrefix]
        internal static bool OnPreVehicleLockedInvoker(in ServerInvocationContext context)
        {
            var player = context.GetPlayer();
            if (player == null)
                return true;

            var vehicle = player.movement.getVehicle();
            if (vehicle == null)
                return true;

            var uPlayer = UnturnedPlayer.FromPlayer(player);
            if (uPlayer.CSteamID.m_SteamID == vehicle.lockedOwner.m_SteamID && vehicle.isLocked)
                return true;

            var limit = LockUtil.GetBestLockLimit(uPlayer);
            var currentCount = LockUtil.GetVehicleLockedCount(uPlayer.CSteamID.m_SteamID);
            if (Plugin.Conf.SyncWithRFGarage)
                currentCount += LockUtil.CountRFGarage(uPlayer.CSteamID.m_SteamID);

            if (currentCount < limit)
            {
                ChatHelper.Say(uPlayer,
                    currentCount + 1 >= limit
                        ? Plugin.Inst.Translate(EResponse.VEHICLE_LOCK_LIMIT_REACH.ToString(), limit)
                        : Plugin.Inst.Translate(EResponse.VEHICLE_LOCK.ToString(), currentCount + 1, limit),
                    Plugin.MsgColor, Plugin.Conf.MessageIconUrl);
                return true;
            }

            ChatHelper.Say(uPlayer, Plugin.Inst.Translate(EResponse.VEHICLE_LOCK_LIMIT_REACH.ToString(), limit),
                Plugin.MsgColor, Plugin.Conf.MessageIconUrl);
            return false;
        }
    }
}