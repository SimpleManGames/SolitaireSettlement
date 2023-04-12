using System;
using DG.Tweening;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardAnimator : MonoBehaviour
    {
        private Vector3 _from;
        private Vector3 _to;
        private Action _onComplete;

        public void AnimateTo(Vector3 from, Vector3 to, float delay, Action onComplete)
        {
            transform.position = from;
            _from = from;
            _to = to;
            _onComplete = onComplete;

            var position = transform.position;
            var tween = DOTween.To(() => position, x => position = x, _to, 1.0f)
                .SetDelay(delay)
                .OnUpdate(() => transform.position = position)
                .OnComplete(() => { _onComplete?.Invoke(); });
            tween.SetId("Card");
        }
    }
}