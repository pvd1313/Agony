namespace Agony.RadialTabs;

using Agony.Common;
using UnityEngine;

internal sealed class Config
{       
    public static int RootIconSize { get; private set; } = 100;
    public static int MinIconSize { get; private set; } = 40;
    public static int MaxRootIconCount { get; private set; } = 10;
    public static double IconForegroundSizeMult { get; private set; } = .65;
    public static int IconSizeReductionDelta { get; private set; } = 12;
    public static double IconSizeReductionMult { get; private set; } = 1;
    public static double AnimationSpeedMult { get; private set; } = 1.5;
    public static double AnimationFadeDistanceMult { get; private set; } = 1;
    public static double AnimationFadePower { get; private set; } = 1;

    static Config()
    {
        var config = new Config();
        ConfigUtil.Read(ref config);
        Validate();
        ConfigUtil.Write(config);
    }

    private static void Validate()
    {
        RootIconSize = Mathf.Clamp(RootIconSize, 64, 256);
        MinIconSize = Mathf.Clamp(MinIconSize, 0, RootIconSize);
        IconForegroundSizeMult = Mathf.Clamp((float)IconForegroundSizeMult, .3f, 1);
        IconSizeReductionDelta = Mathf.Clamp(IconSizeReductionDelta, 0, RootIconSize / 3);
        IconSizeReductionMult = Mathf.Clamp((float)IconSizeReductionMult, .5f, 1);
        AnimationSpeedMult = Mathf.Clamp((float)AnimationSpeedMult, .1f, 10);
        AnimationFadeDistanceMult = Mathf.Clamp((float)AnimationFadeDistanceMult, .1f, 10);
        AnimationFadePower = Mathf.Clamp((float)AnimationFadePower, .1f, 10);
        MaxRootIconCount = Mathf.Clamp(MaxRootIconCount, 3, 24);
    }
}