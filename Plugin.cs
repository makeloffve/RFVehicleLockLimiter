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
            {$"{EResponse.VEHICLE_LOCK_LIMIT_REACH}", "达到锁车上限: {0}, 请提升你的权限!"},
            {$"{EResponse.VEHICLE_LOCK}", "载具上锁情况: {0}/{1}"},
            {"unlock_success", "成功解锁了 {0} 个载具."},
            {"unlock_op_success", "管理员 {0} 成功解锁了地图上所有载具."},
            {"unlock_console_success", "管理员在后台成功解锁了地图上所有载具."},
            {"vehicle_info", "载具实例号:{0}, 载具名:{1}, 使用: /mv {0} 查看载具坐标."},
            {"vehicle_info_op", "载具实例号:{0}, 载具名:{1}, 载具拥有人steamId:{2}."},
            {"vehicle_locked_count", "全服共用{0}被锁的载具."},
            {"vehicle_not_found", "找不到这个实例的载具,请核实."},
            {"vehicle_not_yours", "这个车不属于你."},
            {"vehicle_mark_success", "当前坐标 {0},载具坐标 {1}, 距离: {2}米."},
            {"have_no_locked_vehicle", "没有被锁的车辆."},
            {"cannot_lock_train", "火车是公共载具,禁止上锁."},
            {"cannot_lock", "服务器禁止对此载具上锁,和保存到云车库."},
        };
    }
}