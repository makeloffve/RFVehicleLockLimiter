using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RFVehicleLockLimiter.Commands
{
    internal class WhereVehicleOpCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "wherevehicleop";

        public string Help => "get all locked vehicle position and instanceId";

        public string Syntax => "/wherevehicleop";

        public List<string> Aliases => new List<string> { "wvo" };

        public List<string> Permissions => new List<string> { "xb.vehicle.where.op" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer opPlayer = (UnturnedPlayer)caller;
            try
            {
                opPlayer = ((UnturnedPlayer)caller);

            }
            catch (Exception)
            {
            }

            bool isConsole = null == opPlayer;
            List<InteractableVehicle> vehicles = VehicleManager.vehicles.FindAll(vehicle => vehicle.isLocked);
            if (null != vehicles && vehicles.Count > 0)
            {
                vehicles.ForEach(vehicle =>
                {
                    string vehicleInfo = Plugin.Inst.Translations.Instance.Translate("vehicle_info_op", vehicle.instanceID, vehicle.asset.FriendlyName, vehicle.lockedOwner.m_SteamID);
                   if (!isConsole)
                    {
                        ChatManager.say(opPlayer.CSteamID, vehicleInfo, Color.green);
                    }
                   else
                    {
                        Logger.Log($"[WhereVehicleOpCommand] {vehicleInfo}");
                    }
                });

                if (!isConsole)
                {
                    ChatManager.say(opPlayer.CSteamID, Plugin.Inst.Translations.Instance.Translate("vehicle_locked_count", vehicles.Count), Color.green);
                }
            }
            else
            {
                string tips = Plugin.Inst.Translations.Instance.Translate("have_no_locked_vehicle");
                if (!isConsole)
                {
                    ChatManager.say(opPlayer.CSteamID, tips, Color.red);
                }
                else
                {
                    Logger.Log($"[UnlockAllCommand] {tips}.");
                }
            }
        }
    }
}
