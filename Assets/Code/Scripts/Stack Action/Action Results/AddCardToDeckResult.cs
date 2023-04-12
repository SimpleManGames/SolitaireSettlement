using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AddCardToDeckResult : IStackActionResult
    {
        [field: SerializeField, Required]
        private CardData ProducedCard { get; set; }

        public void OnResult(IEnumerable<Card> relatedCardStack)
        {
            var position = relatedCardStack.ElementAt(0).transform.position;
            CameraManager.Instance.GetPositionOnScreenSpaceCanvas(position, out var pos);
            CardManager.Instance.CreateNewCardRuntimeInfo(ProducedCard, CardRuntimeInfo.CardLocation.Deck, true, pos);
        }
    }
}