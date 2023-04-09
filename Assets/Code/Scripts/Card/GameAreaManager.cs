using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class GameAreaManager : Singleton<GameAreaManager>
    {
        [field: SerializeField, ReadOnly]
        private List<Card> AllCardsInPlay { get; set; }

        [field: SerializeField]
        public GameObject GameAreaCanvas { get; private set; }

        private readonly Queue<GameObject> _toBeDestroyed = new();

        private void Start()
        {
            // Grab the initial cards
            AllCardsInPlay = GameObject.FindGameObjectsWithTag("Card").Select(c => c.GetComponent<Card>()).ToList();
        }

        private void LateUpdate()
        {
            while (_toBeDestroyed.TryDequeue(out var destroy) && destroy != null)
                DeleteCardObject(destroy);
        }

        public void AddCardToGameArea(Card card)
        {
            card.transform.localScale = Vector3.one;
            card.transform.rotation = GameAreaCanvas.transform.rotation;
            AllCardsInPlay.Add(card);
        }

        public void RemoveCardFromGameArea(Card card)
        {
            HandManager.Instance.AddCardToHand(card);
            AllCardsInPlay.Remove(card);
        }

        public void RequestToDeleteCardObject(GameObject cardObject)
        {
            _toBeDestroyed.Enqueue(cardObject);
        }

        private void DeleteCardObject(GameObject cardObject)
        {
            var card = cardObject.GetComponent<Card>();
            card.Stack.RemoveCard(card);
            AllCardsInPlay.Remove(card);
            Destroy(cardObject);
        }
    }
}