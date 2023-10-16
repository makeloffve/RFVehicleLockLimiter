using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFVehicleLockLimiter.Commands
{
    public class UnlockCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "unlock";

        public string Help => "unlock all vehicle owner is yourself";

        public string Syntax => "/unlock";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "xb.unlock"};

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            List<InteractableVehicle> vehicles = VehicleManager.vehicles.FindAll(vehicle => vehicle.lockedOwner.m_SteamID == player.CSteamID.m_SteamID && vehicle.isLocked);
            if(null != vehicles && vehicles.Count > 0)
            {
                vehicles.ForEach(vehicle =>
                {
                    VehicleManager.ServerSetVehicleLock(vehicle, CSteamID.Nil, CSteamID.Nil, isLocked: false);
                });
                ChatManager.say(player.CSteamID, Plugin.Inst.Translations.Instance.Translate("unlock_success", vehicles.Count), Color.magenta);
            }
            else
            {
                ChatManager.say(player.CSteamID, Plugin.Inst.Translations.Instance.Translate("have_no_locked_vehicle"), Color.red);
            }
        }
    }
}
