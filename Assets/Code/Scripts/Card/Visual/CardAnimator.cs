using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardAnimator : MonoBehaviour
    {
        private bool isDrawing = false;

        public IEnumerator AnimateDraw(Vector3 targetPosition)
        {
            isDrawing = true;

            transform.position = DeckManager.Instance.DrawFromPosition;

            var position = transform.position;

            var tween = DOTween.To(() => position, x => position = x, targetPosition, 1.0f)
                .OnUpdate(() => transform.position = position);

            while (tween.IsActive())
                yield return null;

            HandManager.Instance.AddCardToHand(gameObject);

            isDrawing = false;
        }
    }
}