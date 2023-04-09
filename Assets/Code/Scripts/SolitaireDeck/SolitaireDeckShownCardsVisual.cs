using System.Collections.Generic;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DeckShownCardsVisual : MonoBehaviour
    {
        [field: SerializeField]
        private CardFactory CardFactory { get; set; }

        [field: SerializeField]
        private DeckManager DeckManager { get; set; }

        [field: SerializeField]
        private Vector3 ShownCardsOffsetFromDeck { get; set; } = new(10, 0, 0);

        [field: SerializeField]
        private Vector3 ShownCardsOffsetFromEachOther { get; set; } = new(3, 0, 0);

        private List<GameObject> _visualCardObjects = new();

        private const int VISUAL_CARD_OBJECTS_COUNT = 3;

        private void Update()
        {
            for (var i = 0; i < VISUAL_CARD_OBJECTS_COUNT - _visualCardObjects.Count; i++)
            {
                _visualCardObjects.Add(CreateNewEmptyCard());
            }

            _visualCardObjects.ForEach(c =>
            {
                c.SetActive(false);
                c.GetComponent<CardDraggable>().CanBeDragged = false;
                // c.GetComponent<Card>().IsInDeck = true;
            });

            if (!DeckManager.IsShowingCards)
                return;

            var shownCards = DeckManager.CurrentlyVisibleShownCards;

            for (var i = shownCards.Count - 1; i >= 0; i--)
            {
                if (shownCards[i] == null)
                    continue;

                var visualCardObject = _visualCardObjects[i].GetComponent<Card>();
                visualCardObject.UpdateCardData(shownCards[i]);
                visualCardObject.gameObject.transform.position =
                    transform.position + ShownCardsOffsetFromDeck + ShownCardsOffsetFromEachOther * i;
                visualCardObject.gameObject.SetActive(true);
            }

            _visualCardObjects[^1].GetComponent<CardDraggable>().CanBeDragged = true;
        }

        public void MoveTopShownCardToGame()
        {
            _visualCardObjects[^1].transform.SetParent(transform.parent);
            // _visualCardObjects[^1].GetComponent<Card>().IsInDeck = false;
            _visualCardObjects[^1] = CreateNewEmptyCard();
        }

        private GameObject CreateNewEmptyCard()
        {
            var newCardObject = CardFactory.CreateEmptyCardObject();
            newCardObject.SetActive(false);
            return newCardObject;
        }
    }
}