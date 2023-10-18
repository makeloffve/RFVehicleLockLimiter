using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RFVehicleLockLimiter.Commands
{
    internal class MarkVehicleCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "markvehicle";

        public string Help => "Usage: /mv [instaneId], id must be interge";

        public string Syntax => "/markvehicle";

        public List<string> Aliases => new List<string> { "mv" };

        public List<string> Permissions => new List<string> { "xb.vehicle.mark" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (null == player || command.Length < 1) return;

            if (uint.TryParse(command[0], out uint vehicleInstanceId))
            {
                InteractableVehicle vehicle = VehicleManager.findVehicleByNetInstanceID(vehicleInstanceId);
                if(null == vehicle)
                {
                    ChatManager.say(player.CSteamID, Plugin.Inst.Translations.Instance.Translate("vehicle_not_found"), Color.red);
                    return;
                }
                if(!player.IsAdmin && !player.CSteamID.Equals(vehicle.lockedOwner))
                {
                    ChatManager.say(player.CSteamID, Plugin.Inst.Translations.Instance.Translate("vehicle_not_yours"), Color.red);
                    return;
                }
                Vector3 currrentPosition = player.Position;
                Vector3 vehiclePosotion = vehicle.transform.position;
                int distance = (int)Vector3.Distance(currrentPosition, vehiclePosotion);
                ChatManager.say(player.CSteamID, Plugin.Inst.Translations.Instance.Translate("vehicle_mark_success", currrentPosition, vehiclePosotion, distance), Color.magenta);
            }
            else
            {
                ChatManager.say(player.CSteamID, Help, Color.yellow);
            }
        }
    }
}
