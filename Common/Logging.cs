namespace Common;

using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Logging;

public class Logging
{
    internal static ManualLogSource Logger;

    public static void Initialize(ManualLogSource logSource)
    {
        Logger = logSource;
    }
}
