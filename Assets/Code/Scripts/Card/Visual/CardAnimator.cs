using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardAnimator : MonoBehaviour
    {
        public void ExternalAnimateDraw(Vector3 from, Vector3 target, Action onComplete)
        {
            StartCoroutine(AnimateDraw(from, target, onComplete));
        }

        public IEnumerator AnimateDraw(Vector3 from, Vector3 target, Action onComplete)
        {
            transform.position = from;

            var position = transform.position;

            var tween = DOTween.To(() => position, x => position = x, target, 1.0f)
                .OnUpdate(() => transform.position = position);

            while (tween.IsActive())
                yield return null;

            onComplete.Invoke();
        }
    }
}