using System.Collections.Generic;
using System.Reflection;
using Agony.Common;
using System;

namespace Agony.AssetTools.Wrappers
{
    public static class TooltipFactoryWrapper
    {
        private static readonly FieldInfo CachedTechTypeFieldInfo = typeof(CachedEnumString<TechType>).GetField("valueToString", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly Dictionary<TechType, string> techTypeTooltips = (Dictionary<TechType, string>)CachedTechTypeFieldInfo.GetValue(TooltipFactory.techTypeTooltipStrings);

        public static void RegisterTech(TechType techType)
        {
            if (techTypeTooltips.ContainsKey(techType))
            {
                Logger.Exception(new ArgumentException($"TooltipFactory already contains TechType '{techType}'."));
            }
            techTypeTooltips[techType] = "Tooltip_" + techType.AsString();
        }
    }
}