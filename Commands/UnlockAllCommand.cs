using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RFVehicleLockLimiter.Commands
{
    public class UnlockAllCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "unlockall";

        public string Help => "unlock all vehicle in the map";

        public string Syntax => "/unlockall";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "xb.unlock.op" };

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
            List<InteractableVehicle> vehicles = VehicleManager.vehicles.FindAll(vehicle => vehicle.isLocked);
            if (null != vehicles && vehicles.Count > 0)
            {
                vehicles.ForEach(vehicle =>
                {
                    VehicleManager.ServerSetVehicleLock(vehicle, CSteamID.Nil, CSteamID.Nil, isLocked: false);
                });
                string tips = Plugin.Inst.Translations.Instance.Translate("unlock_console_success");
                if (!isConsole)
                {
                    tips = Plugin.Inst.Translations.Instance.Translate("unlock_op_success", opPlayer.DisplayName);
                }

                ChatManager.say(tips, Color.magenta);
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
