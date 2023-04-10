using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardManager : Singleton<CardManager>
    {
        [field: SerializeField, ReadOnly]
        public List<CardRuntimeInfo> AllCardsInfo { get; private set; }

        [field: SerializeField]
        public GameObject GameAreaCanvas { get; private set; }

        [field: SerializeField]
        public GameObject HandCanvas { get; private set; }

        [field: SerializeField]
        public Camera GameCamera { get; private set; }

        [field: SerializeField]
        private float DurationBetweenCardDiscard { get; set; } = 0.5f;

        private readonly Queue<GameObject> _toBeDestroyed = new();

        private void Start()
        {
            // Grab the initial cards
            AllCardsInfo = GameObject.FindGameObjectsWithTag("Card")
                .Select(c => c.GetComponent<Card>().Info)
                .ToList();
        }

        private void LateUpdate()
        {
            while (_toBeDestroyed.TryDequeue(out var destroy) && destroy != null)
                DeleteCardObject(destroy);
        }

        public void CreateNewCardRuntimeInfo(CardData data, CardRuntimeInfo.CardLocation location)
        {
            AllCardsInfo.Add(new CardRuntimeInfo()
            {
                Data = data,
                Location = location
            });
        }

        public void RequestToDeleteCardObject(GameObject cardObject)
        {
            _toBeDestroyed.Enqueue(cardObject);
        }

        private void DeleteCardObject(GameObject cardObject)
        {
            var card = cardObject.GetComponent<Card>();
            card.Stack?.RemoveCard(card);
            card.Info.SetCardLocation(CardRuntimeInfo.CardLocation.Delete);
            Destroy(cardObject);
        }
    }
}