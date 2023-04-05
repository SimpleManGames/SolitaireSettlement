using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardManager : Singleton<CardManager>
    {
        [field: Title("References")]
        [field: SerializeField]
        private DeckManager DeckManager { get; set; }

        [field: SerializeField, ReadOnly]
        private List<Card> AllCards { get; set; }

        [field: SerializeField]
        private GameObject CardCanvas { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<CardData> ToBeAddedCards { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<Card> ToBeDeletedCards { get; set; }

        [field: Title("Settings")]
        [field: SerializeField, AssetsOnly]
        private GameObject CardPrefab { get; set; }

        private void Start()
        {
            // Grab the initial cards
            AllCards = GameObject.FindGameObjectsWithTag("Card").Select(c => c.GetComponent<Card>()).ToList();

            ToBeAddedCards = new List<CardData>();
            ToBeDeletedCards = new List<Card>();
        }

        private void Update()
        {
            for (var i = 0; i < AllCards.Count; i++) AllCards[i].GetComponent<CardRenderer>().Index = i;

            foreach (var newCard in ToBeAddedCards) AddCardToDeck(newCard);
            foreach (var cardObject in ToBeDeletedCards) DeleteCard(cardObject);

            ToBeAddedCards.Clear();
            ToBeDeletedCards.Clear();
        }

        /// <summary>
        /// Requests to create a new Card with the related Data.
        /// </summary>
        /// <param name="newCard">Data to base the new Card off of.</param>
        public void RequestToAddCard(CardData newCard)
        {
            ToBeAddedCards.Add(newCard);
        }

        public void RequestToDeleteCard(Card cardObject)
        {
            ToBeDeletedCards.Add(cardObject);
        }

        private void AddCardToDeck(CardData data)
        {
            DeckManager.AddCardToDeck(data);
        }

        private void DeleteCard(Card cardObject)
        {
            cardObject.Stack.RemoveCard(cardObject);
            AllCards.Remove(cardObject);
            Destroy(cardObject.gameObject);
        }
    }
}