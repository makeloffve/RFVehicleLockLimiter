using Rocket.API;

namespace RFVehicleLockLimiter
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Enabled;
        public bool SyncWithRFGarage;
        public uint DefaultVehicleLockLimit;

        public void LoadDefaults()
        {
            Enabled = true;
            SyncWithRFGarage = false;
            DefaultVehicleLockLimit = 1;
        }
    }
}