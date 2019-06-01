using System.Reflection;
using Agony.Common.Animation;
using UnityEngine;
using System;

namespace Agony.Defabricator
{
    internal sealed class IconColorAnimation : ObjectAnimation<uGUI_ItemIcon>
    {
        private static readonly FieldInfo foregroundColorNormalFieldInfo = typeof(uGUI_ItemIcon).GetField("foregroundColorNormal", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo backgroundColorNormalFieldInfo = typeof(uGUI_ItemIcon).GetField("backgroundColorNormal", BindingFlags.NonPublic | BindingFlags.Instance);

        private float _timeLeft;
        private Color _frontColor;
        private Color _backColor;

        public IconColorAnimation(Color frontColor, Color backColor, float duration)
        {
            if (duration <= 0) throw new ArgumentException("duration <= 0");

            _frontColor = frontColor;
            _backColor = backColor;
            _timeLeft = duration;
        }

        protected override void OnStart(uGUI_ItemIcon actor) { }

        protected override bool OnUpdate(uGUI_ItemIcon actor)
        {
            if (!actor) return false;

            var foreground = (Color)foregroundColorNormalFieldInfo.GetValue(actor);
            var background = (Color)backgroundColorNormalFieldInfo.GetValue(actor);

            var lerp = Mathf.Clamp01(Time.deltaTime / _timeLeft);
            var resultFront = Color.Lerp(foreground, _frontColor, lerp);
            var resultBack = Color.Lerp(background, _backColor, lerp);

            actor.SetForegroundColors(resultFront, resultFront, resultFront);
            actor.SetBackgroundColors(resultBack, resultBack, resultBack);

            _timeLeft -= Time.deltaTime;
            return _timeLeft > 0;
        }

        protected override void OnStop(uGUI_ItemIcon actor) { }
    }
}
