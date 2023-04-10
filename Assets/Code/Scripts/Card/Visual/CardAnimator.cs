using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardAnimator : MonoBehaviour
    {
        public IEnumerator AnimateDraw(Vector3 targetPosition)
        {
            transform.position = DeckManager.Instance.DrawFromPosition;

            var position = transform.position;

            var tween = DOTween.To(() => position, x => position = x, targetPosition, 1.0f)
                .OnUpdate(() => transform.position = position);

            while (tween.IsActive())
                yield return null;

            HandManager.Instance.AddCardToHand(gameObject);
        }
    }
}