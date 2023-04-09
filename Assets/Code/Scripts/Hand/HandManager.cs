using System;
using System.Collections.Generic;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class HandManager : Singleton<HandManager>
    {
        [field: SerializeField]
        private RectTransform CardsInHandContainer { get; set; }

        [field: SerializeField, ReadOnly]
        private List<Card> CardsInHand { get; set; }

        [field: SerializeField]
        private GameObject DrawnCardTargetPosition { get; set; }

        [field: SerializeField]
        public Vector3 InHandScale { get; private set; } = new Vector3(15, 15, 1);

        private void Update()
        {
            DrawnCardTargetPosition.transform.SetAsLastSibling();
        }

        public void DrawCardToHand(CardData cardData)
        {
            var card = CardFactory.Instance.CreateCardObjectFromDataInUICanvas(cardData);
            var cardAnimator = card.GetComponent<CardAnimator>();
            StartCoroutine(cardAnimator.AnimateDraw(DrawnCardTargetPosition.transform.position));
        }

        public void AddCardToHand(Card card)
        {
            CardsInHand.Add(card);
            SetCardPropertiesWhileInHand(card.gameObject);
        }

        public void AddCardToHand(GameObject cardObject)
        {
            var card = cardObject.GetComponent<Card>();
            AddCardToHand(card);
        }

        public void RemoveCardFromHand(Card cardObject)
        {
            CardsInHand.Remove(cardObject);
        }

        private void SetCardPropertiesWhileInHand(GameObject gameObject)
        {
            gameObject.transform.SetParent(CardsInHandContainer);
            gameObject.transform.localScale = InHandScale;
        }
    }
}