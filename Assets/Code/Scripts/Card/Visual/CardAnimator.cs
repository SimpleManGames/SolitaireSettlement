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

        public void AnimateTo(Vector3 from, Vector3 to, Action onComplete)
        {
            transform.position = from;
            _from = from;
            _to = to;
            _onComplete = onComplete;
            
            var position = transform.position;
            DOTween.To(() => position, x => position = x, _to, 1.0f)
                .OnUpdate(() => transform.position = position)
                .OnComplete(() =>
                {
                    _onComplete?.Invoke();
                    Debug.Log("Card Animation Complete!");
                });
        }
    }
}