using System;
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
        private Vector3 ShownCardsOffsetFromEachother { get; set; } = new(3, 0, 0);

        private List<GameObject> _visualCardObjects = new();

        private const int VISUAL_CARD_OBJECTS_COUNT = 3;

        private void Awake()
        {
            for (var i = 0; i < VISUAL_CARD_OBJECTS_COUNT; i++)
            {
                var newCardObject = CardFactory.CreateEmptyCardObject();
                newCardObject.SetActive(false);
                _visualCardObjects.Add(newCardObject);
            }
        }

        private void Update()
        {
            _visualCardObjects.ForEach(c => c.SetActive(false));
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
                    transform.position + ShownCardsOffsetFromDeck + ShownCardsOffsetFromEachother * i;
                visualCardObject.gameObject.SetActive(true);
            }
        }
    }
}