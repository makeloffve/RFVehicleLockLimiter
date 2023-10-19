using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RFVehicleLockLimiter.Commands
{
    internal class CleanSpecialVehiclesCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "cleanspecialvehicles";

        public string Help => "clean special vehicles in RFGargae Blacklists";

        public string Syntax => "/cleanspecialvehicles";

        public List<string> Aliases => new List<string> { "csvs" };

        public List<string> Permissions => new List<string> { "xb.vehicle.special.clean" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer opPlayer = null;
            try
            {
                opPlayer = ((UnturnedPlayer)caller);

            }
            catch (Exception)
            {
            }

            bool isConsole = null == opPlayer;
            List<InteractableVehicle> vehicles = VehicleManager.vehicles.FindAll(
                vehicle => RFGarage.Plugin.Conf.Blacklists.Any(
                    x => x.Type == RFGarage.Enums.EBlacklistType.VEHICLE 
                    && x.IdList.Contains(vehicle.id))
                );
            if (null != vehicles && vehicles.Count > 0)
            {
                vehicles.ForEach(vehicle =>
                {
                    VehicleManager.askVehicleDestroy(vehicle);
                });

                ChatManager.say(Plugin.Inst.Translations.Instance.Translate("clean_special_vehicles_success"), UnityEngine.Color.yellow);
                Logger.Log($"[CleanSpecialVehiclesCommand] clean special vehicles success");
            }
            else
            {
                if (!isConsole)
                {
                    ChatManager.say(opPlayer.CSteamID, Plugin.Inst.Translations.Instance.Translate("have_no_special_vehicle"), UnityEngine.Color.red);
                }
                Logger.Log($"[CleanSpecialVehiclesCommand] there is no special vehicles to clean.....");
            }
        }
    }
}
