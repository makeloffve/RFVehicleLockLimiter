using HarmonyLib;
using RFRocketLibrary.Helpers;
using RFVehicleLockLimiter.Enums;
using RFVehicleLockLimiter.Models;
using RFVehicleLockLimiter.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;
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
                return false;

            if (Plugin.Conf.IgnoreAdmins && player.channel.owner.isAdmin)
                return true;

            var vehicle = player.movement.getVehicle();
            if (vehicle == null)
                return false;

            if (Plugin.Conf.IgnoredIDs.Contains(new Vehicle { Id = vehicle.asset.id }))
                return true;
            
            UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(player);
            if (uPlayer == null)
                return false;
            // 如果是火车并且RFGarage里面禁止保存火车,那么禁止上锁
            if (!RFGarage.Plugin.Conf.AllowTrain && vehicle.asset.engine == EEngine.TRAIN)
            {
                ChatManager.say(uPlayer.CSteamID, Plugin.Inst.Translations.Instance.Translate("cannot_lock_train"), Color.red);
                return false;
            }
            // 如果是RFGarage里面不允许存入车库的车辆,那么禁止上锁
            if (RFGarage.Plugin.Conf.Blacklists.Any(x =>
                    x.Type == RFGarage.Enums.EBlacklistType.VEHICLE && !uPlayer.HasPermission(x.BypassPermission) &&
                    x.IdList.Contains(vehicle.id)))
            {
                ChatManager.say(uPlayer.CSteamID, Plugin.Inst.Translations.Instance.Translate("cannot_lock"), Color.red);
                return false;
            }
           
            if (uPlayer.CSteamID.m_SteamID == vehicle.lockedOwner.m_SteamID && vehicle.isLocked)
                return true;

            var limit = LockUtil.GetBestLockLimit(uPlayer);
            var currentCount = LockUtil.GetVehicleLockedCount(uPlayer.CSteamID.m_SteamID);
            if (Plugin.Conf.SyncWithRFGarage)
                currentCount += LockUtil.CountRFGarage(uPlayer.CSteamID.m_SteamID);

            if (currentCount < limit)
            {
                ChatHelper.Say(uPlayer,
                    currentCount + 1 == limit
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