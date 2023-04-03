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
        
        [field: SerializeField, ReadOnly]
        private List<Card> AllCards { get; set; }

        [field: SerializeField]
        private GameObject CardCanvas { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<CardData> ToBeCreatedCards { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<Card> ToBeDeletedCards { get; set; }

        [field: Title("Settings")]
        [field: SerializeField, AssetsOnly]
        private GameObject CardPrefab { get; set; }

        private void Start()
        {
            // Grab the initial cards
            AllCards = GameObject.FindGameObjectsWithTag("Card").Select(c => c.GetComponent<Card>()).ToList();

            ToBeCreatedCards = new List<CardData>();
            ToBeDeletedCards = new List<Card>();
        }

        private void Update()
        {
            foreach (var newCard in ToBeCreatedCards) CreateNewCard(newCard);
            foreach (var cardObject in ToBeDeletedCards) DeleteCard(cardObject);
            ToBeDeletedCards.Clear();
        }

        /// <summary>
        /// Requests to create a new Card with the related Data.
        /// </summary>
        /// <param name="newCard">Data to base the new Card off of.</param>
        public void RequestToAddCard(CardData newCard)
        {
            ToBeCreatedCards.Add(newCard);
        }

        public void RequestToDeleteCard(Card cardObject)
        {
            ToBeDeletedCards.Add(cardObject);
        }

        private void CreateNewCard(CardData data)
        {
            var newCardObject = Instantiate(CardPrefab, CardCanvas.transform);
            var newCardComponent = newCardObject.GetComponent<Card>();
            newCardComponent.Data = data;
        }

        private void DeleteCard(Card cardObject)
        {
            cardObject.Stack.RemoveCard(cardObject);
            AllCards.Remove(cardObject);
            Destroy(cardObject.gameObject);
        }
    }
}