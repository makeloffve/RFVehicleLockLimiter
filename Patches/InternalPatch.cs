using HarmonyLib;
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

            var uPlayer = UnturnedPlayer.FromPlayer(player);
            var limit = LockUtil.GetBestLockLimit(uPlayer);
            var currentCount = LockUtil.GetVehicleLockedCount(uPlayer.CSteamID.m_SteamID);
            if (Plugin.Conf.SyncWithRFGarage)
                currentCount += RFGarage.DatabaseManagers.GarageManager.Count(uPlayer.CSteamID.m_SteamID);
            
            if (currentCount < limit) 
                return true;
            
            UnturnedChat.Say(uPlayer, Plugin.Inst.Translate(EResponse.VEHICLE_LOCK_LIMIT_REACH.ToString(), limit), Color.green);
            return false;

        }
    }
}