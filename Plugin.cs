using HarmonyLib;
using RFVehicleLockLimiter.Enums;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RFVehicleLockLimiter
{
    public class Plugin : RocketPlugin<Configuration>
    {
        private static int Major = 1;
        private static int Minor = 0;
        private static int Patch = 1;

        public static Plugin Inst;
        public static Configuration Conf;
        internal static Color MsgColor;
        private Harmony _harmony;

        protected override void Load()
        {
            Inst = this;
            Conf = Configuration.Instance;
            if (Conf.Enabled)
            {
                MsgColor = UnturnedChat.GetColorFromName(Conf.MessageColor, Color.green);
                _harmony = new Harmony("RFVehicleLockLimiter.Patches");
                _harmony.PatchAll();
            }
            else
                Logger.LogWarning($"[{Name}] Library: DISABLED");

            Logger.LogWarning($"[{Name}] Library loaded successfully!");
            Logger.LogWarning($"[{Name}] {Name} v{Major}.{Minor}.{Patch}");
            Logger.LogWarning($"[{Name}] Made with 'rice' by RiceField Plugins!");
        }

        protected override void Unload()
        {
            if (Conf.Enabled)
            {
                _harmony.UnpatchAll("RFVehicleLockLimiter.Patches");
            }

            Conf = null;
            Inst = null;

            Logger.LogWarning($"[{Name}] Plugin unloaded successfully!");
        }

        public override TranslationList DefaultTranslations => new()
        {
            {$"{EResponse.VEHICLE_LOCK_LIMIT_REACH}", "You have reach maximum vehicle lock limit! Max: {0} vehicles"},
            {$"{EResponse.VEHICLE_LOCK}", "Used vehicle lock: {0}/{1}"},
        };
    }
}