using Rocket.API;

namespace RFVehicleLockLimiter
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Enabled;
        public string MessageColor;
        public string MessageIconUrl;
        public bool SyncWithRFGarage;
        public uint DefaultVehicleLockLimit;

        public void LoadDefaults()
        {
            Enabled = true;
            MessageColor = "green";
            MessageIconUrl = "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/plugin/RFVault/RFVault.png";
            SyncWithRFGarage = false;
            DefaultVehicleLockLimit = 1;
        }
    }
}