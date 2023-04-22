using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace SolitaireSettlement
{
    public class HandManager : Singleton<HandManager>
    {
        [field: ShowInInspector, ReadOnly]
        private List<CardRuntimeInfo> _cardsInHand = new();

        private List<CardRuntimeInfo> CardsInHand =>
            _cardsInHand = CardManager.Instance.AllCardsInfo
                .Where(c => c.Location == CardRuntimeInfo.CardLocation.Hand)
                .ToList();

        [field: SerializeField]
        public GameObject HandContainer { get; private set; }

        [field: SerializeField]
        private GameObject DrawnCardTarget { get; set; }

        public Vector3 DrawnCardTargetPosition => DrawnCardTarget.transform.position;

        [field: SerializeField]
        private float DurationBetweenCardDiscard { get; set; } = 0.5f;

        private void Update()
        {
            DrawnCardTarget.transform.SetAsLastSibling();
        }

        public void SendHandCardsToDiscard()
        {
            DiscardCards();
        }

        private void DiscardCards()
        {
            for (var i = CardsInHand.Count - 1; i >= 0; i--)
            {
                // CardsInHand[i].SetPosition(CardsInHand[i].RelatedGameObject.transform.position);
                CardsInHand[i].SetCardLocation(CardRuntimeInfo.CardLocation.Discard, true, i * DurationBetweenCardDiscard);
            }
        }
    }
}