using System.Reflection;
using System;

namespace Agony.Common.Reflection
{
    public class uGUI_CraftingMenuReflector
    {
        private static readonly FieldInfo clientFieldInfo = typeof(uGUI_CraftingMenu).GetField("_client", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo interactableFieldInfo = typeof(uGUI_CraftingMenu).GetField("interactable", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo iconsFieldInfo = typeof(uGUI_CraftingMenu).GetField("icons", BindingFlags.NonPublic | BindingFlags.Instance);

        public static ITreeActionReceiver GetClient(uGUI_CraftingMenu menu)
        {
            if (menu == null) throw new ArgumentNullException("menu is null");
            return (ITreeActionReceiver)clientFieldInfo.GetValue(menu);
        }

        public static bool GetInteractable(uGUI_CraftingMenu menu)
        {
            if (menu == null) throw new ArgumentNullException("menu is null");
            return (bool)interactableFieldInfo.GetValue(menu);
        }

        public static uGUI_CraftNode GetIcons(uGUI_CraftingMenu menu)
        {
            if (menu == null) throw new ArgumentNullException("menu is null");
            return (uGUI_CraftNode)iconsFieldInfo.GetValue(menu);
        }
    }
}