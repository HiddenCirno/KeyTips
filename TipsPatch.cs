using EFT;
using EFT.Communications;
using EFT.Interactive;
using EFT.InventoryLogic;
using HarmonyLib;

namespace KeyTips
{
    [HarmonyPatch(typeof(GetActionsClass), nameof(GetActionsClass.smethod_5))]
    public class VulcanCore_GetActionsClass_LockedDoorHint_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(GamePlayerOwner owner, WorldInteractiveObject worldInteractiveObject, ref ActionsReturnClass __result)
        {
            if (worldInteractiveObject.DoorState == EDoorState.Locked && !string.IsNullOrEmpty(worldInteractiveObject.KeyId))
            {
                if (__result != null && !string.IsNullOrEmpty(__result.Error))
                {
                    string keyName = LocaleManagerClass.LocaleManagerClass.method_4(worldInteractiveObject.KeyId + " Name");

                    if (string.IsNullOrEmpty(keyName) || keyName == (worldInteractiveObject.KeyId + " Name"))
                    {
                        keyName = worldInteractiveObject.KeyId;
                    }

                    NotificationManagerClass.DisplayMessageNotification(
                        string.Format("door_locked".i18n(), keyName),
                        ENotificationDurationType.Default,
                        ENotificationIconType.Alert,
                        null
                    );
                }
            }
        }
    }

    [HarmonyPatch(typeof(GetActionsClass), nameof(GetActionsClass.smethod_13))]
    public class VulcanCore_KeycardDoor_DirectNotify_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(GamePlayerOwner owner, KeycardDoor door, bool isProxy, ref ActionsReturnClass __result)
        {
            if (door.DoorState == EDoorState.Locked && !string.IsNullOrEmpty(door.KeyId))
            {
                string keyName = LocaleManagerClass.LocaleManagerClass.method_4(door.KeyId + " Name");

                if (string.IsNullOrEmpty(keyName) || keyName == (door.KeyId + " Name"))
                {
                    keyName = door.KeyId;
                }

                NotificationManagerClass.DisplayMessageNotification(
                    string.Format("keycard_door_locked".i18n(), keyName),
                    ENotificationDurationType.Default,
                    ENotificationIconType.Alert,
                    null
                );
            }
        }
    }

    [HarmonyPatch(typeof(KeycardDoor), "UnlockOperation")]
    public class VulcanCore_UnlockOperation_KeyCard_Notify_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix(KeyComponent key, ref GStruct156<KeyInteractionResultClass> __result)
        {
            if (__result.Failed || __result.Value == null || !__result.Value.Succeed)
            {
                return;
            }

            string keyName = LocaleManagerClass.LocaleManagerClass.method_4(key.Template.KeyId + " Name");
            int maxUsage = key.Template.MaximumNumberOfUsage;
            string usageTip = maxUsage > 0 ? string.Format("key_and_keycard_usage".i18n(), maxUsage - key.NumberOfUsages, maxUsage) : "";

            NotificationManagerClass.DisplayMessageNotification(
                string.Format("key_and_keycard_using".i18n(), keyName, usageTip),
                ENotificationDurationType.Default,
                ENotificationIconType.Default,
                null
            );

            if (maxUsage > 0 && key.NumberOfUsages >= maxUsage)
            {
                NotificationManagerClass.DisplayMessageNotification(
                    string.Format("key_and_keycard_broken".i18n(), keyName),
                    ENotificationDurationType.Default,
                    ENotificationIconType.Alert,
                    null
                );
            }
        }
    }

    [HarmonyPatch(typeof(WorldInteractiveObject), "UnlockOperation")]
    public class VulcanCore_UnlockOperation_World_Notify_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix(KeyComponent key, ref GStruct156<KeyInteractionResultClass> __result)
        {
            if (__result.Failed || __result.Value == null || !__result.Value.Succeed)
            {
                return;
            }

            string keyName = LocaleManagerClass.LocaleManagerClass.method_4(key.Template.KeyId + " Name");
            int maxUsage = key.Template.MaximumNumberOfUsage;
            string usageTip = maxUsage > 0 ? string.Format("key_and_keycard_usage".i18n(), maxUsage - key.NumberOfUsages, maxUsage) : "";

            NotificationManagerClass.DisplayMessageNotification(
                string.Format("key_and_keycard_using".i18n(), keyName, usageTip),
                ENotificationDurationType.Default,
                ENotificationIconType.Default,
                null
            );

            if (maxUsage > 0 && key.NumberOfUsages >= maxUsage)
            {
                NotificationManagerClass.DisplayMessageNotification(
                    string.Format("key_and_keycard_broken".i18n(), keyName),
                    ENotificationDurationType.Default,
                    ENotificationIconType.Alert,
                    null
                );
            }
        }
    }
}