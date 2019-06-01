using Agony.Common.Animation;
using UnityEngine;

namespace Agony.RadialTabs
{
    internal sealed class IconMovingAnimation : ObjectAnimation<uGUI_ItemIcon>
    {
        private float _speed;
        private float _fadeDistance;
        private Vector2 _targetPosition;

        public IconMovingAnimation(float speed, float fadeDistance, Vector2 targetPosition)
        {
            _speed = speed;
            _targetPosition = targetPosition;
            _fadeDistance = fadeDistance;
        }

        protected override void OnStart(uGUI_ItemIcon actor)
        {
            if (!actor) return;
            actor.SetAlpha(0, 0, 0);
            actor.enabled = false;
        }

        protected override bool OnUpdate(uGUI_ItemIcon actor)
        {
            if (!actor) return false;

            var pos = actor.rectTransform.anchoredPosition;
            pos = Vector2.MoveTowards(pos, _targetPosition, _speed * Time.deltaTime);
            actor.rectTransform.anchoredPosition = pos;

            var distance = (pos - _targetPosition).magnitude;
            var alpha = Mathf.Max(1 - distance / _fadeDistance, 0);
            alpha = Mathf.Pow(alpha, (float)Config.AnimationFadePower);
            actor.SetAlpha(alpha, alpha, alpha);         

            if (distance < 1e-3f)
            {
                actor.enabled = true;
                return false;
            }
            return true;
        }

        protected override void OnStop(uGUI_ItemIcon actor)
        {
            if (!actor) return;
            actor.rectTransform.anchoredPosition = _targetPosition;
            actor.SetAlpha(1, 1, 1);
            actor.enabled = true;
        }
    }
}