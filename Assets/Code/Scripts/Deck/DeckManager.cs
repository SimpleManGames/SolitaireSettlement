using System.Collections;
using System.Collections.Generic;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DeckManager : Singleton<DeckManager>
    {
        [field: ShowInInspector, ReadOnly]
        private List<CardData> CardsInDeck { get; set; }

        private bool HasCardsInDeck => CardsInDeck.Count > 0;

        [field: SerializeField]
        public int CardsDrawnPerRound { get; private set; }

        [field: SerializeField]
        private float DurationBetweenCardDraw { get; set; } = 0.5f;

        [field: SerializeField]
        public GameObject DrawFromPositionObject { get; set; }

        public Vector3 DrawFromPosition => DrawFromPositionObject.transform.position;

        public override void Awake()
        {
            base.Awake();

            CardsInDeck = new List<CardData>();
        }

        public void StartCoroutineDrawCards()
        {
            StartCoroutine(DrawCards());
        }

        public void AddCardToDeck(CardData data)
        {
            CardsInDeck.Add(data);
        }

        public void AddCardsToDeck(CardData[] dataCollection)
        {
            foreach (var data in dataCollection)
                AddCardToDeck(data);
        }

        private IEnumerator DrawCards()
        {
            var i = 0;
            var hasEnoughCards = CardsInDeck.Count >= CardsDrawnPerRound;
            while (i < CardsDrawnPerRound)
            {
                if (CardsInDeck.Count <= 0)
                    break;

                var cardDataToDraw = CardsInDeck[0];
                HandManager.Instance.DrawCardToHand(cardDataToDraw);
                CardsInDeck.Remove(cardDataToDraw);
                i++;
                yield return new WaitForSeconds(DurationBetweenCardDraw);
            }
        }
    }
}