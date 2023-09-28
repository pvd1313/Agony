namespace Agony.Defabricator;

using UnityEngine;

partial class CrafterFX
{
    public static class Config
    {
        public static Color BeamColor { get; private set; } =      new Color(0.8f, 0.2f, 0.3f, 1.0f);
        public static Color BeamAlphaColor { get; private set; } = new Color(0.9f, 0.4f, 0.4f, 0.4f);
        public static Color GhostColor { get; private set; } =     new Color(0.9f, 0.6f, 0.6f, 1.0f);

        public static float BeamFrequency { get; private set; } = 60;
    }
}