using System.Collections.Generic;
using RFVehicleLockLimiter.Models;
using Rocket.API;

namespace RFVehicleLockLimiter
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Enabled;
        public string MessageColor;
        public string MessageIconUrl;
        public bool IgnoreAdmins;
        public uint DefaultVehicleLockLimit;
        public bool SyncWithRFGarage;
        public HashSet<Vehicle> IgnoredIDs;

        public void LoadDefaults()
        {
            Enabled = true;
            MessageColor = "green";
            MessageIconUrl = "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/plugin/RFVault/RFVault.png";
            IgnoreAdmins = true;
            DefaultVehicleLockLimit = 1;
            SyncWithRFGarage = false;
            IgnoredIDs = new HashSet<Vehicle>
            {
                new Vehicle { Id = 1 },
                new Vehicle { Id = 2 },
            };
        }
    }
}