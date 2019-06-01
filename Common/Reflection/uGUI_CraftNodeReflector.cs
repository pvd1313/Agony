using System.Reflection;
using System;

namespace Agony.Common.Reflection
{
    public static class uGUI_CraftNodeReflector
    {
        private static readonly FieldInfo viewFieldInfo = typeof(uGUI_CraftNode).GetField("view", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly PropertyInfo indexPropertyInfo = typeof(uGUI_CraftNode).GetProperty("index", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly PropertyInfo visiblePropertyInfo = typeof(uGUI_CraftNode).GetProperty("visible", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo isLockedInHierarchyMethodInfo = typeof(uGUI_CraftNode).GetMethod("IsLockedInHierarchy", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool GetVisible(uGUI_CraftNode node)
        {
            if (node == null) throw new ArgumentNullException("node is null");
            return (bool)visiblePropertyInfo.GetValue(node, null);
        }

        public static int GetIndex(uGUI_CraftNode node)
        {
            if (node == null) throw new ArgumentNullException("node is null");
            return (int)indexPropertyInfo.GetValue(node, null);
        }

        public static uGUI_CraftingMenu GetView(uGUI_CraftNode node)
        {
            if (node == null) throw new ArgumentNullException("node is null");
            return (uGUI_CraftingMenu)viewFieldInfo.GetValue(node);
        }

        public static bool IsLockedInHierarchy(uGUI_CraftNode node)
        {
            if (node == null) throw new ArgumentNullException("node is null");
            return (bool)isLockedInHierarchyMethodInfo.Invoke(node, null);
        }
    }
}