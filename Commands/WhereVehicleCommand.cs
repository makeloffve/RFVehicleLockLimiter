using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RFVehicleLockLimiter.Commands
{
    internal class WhereVehicleCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "wherevehicle";

        public string Help => "get your locked vehicle position and instanceId";

        public string Syntax => "/wherevehicle";

        public List<string> Aliases => new List<string> { "wv" };

        public List<string> Permissions => new List<string> { "xb.vehicle.where" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            List<InteractableVehicle> vehicles = VehicleManager.vehicles.FindAll(vehicle => vehicle.lockedOwner.m_SteamID == player.CSteamID.m_SteamID && vehicle.isLocked);
            if (null != vehicles && vehicles.Count > 0)
            {
                vehicles.ForEach(vehicle =>
                {
                    ChatManager.say(player.CSteamID, Plugin.Inst.Translations.Instance.Translate("vehicle_info", vehicle.instanceID, vehicle.asset.FriendlyName), Color.magenta);
                });
            }
            else
            {
                ChatManager.say(player.CSteamID, Plugin.Inst.Translations.Instance.Translate("have_no_locked_vehicle"), Color.red);
            }
        }
    }
}
