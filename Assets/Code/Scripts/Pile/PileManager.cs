using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class PileManager : SerializedMonoBehaviour
    {
        [field: SerializeField, AssetsOnly] private GameObject CardPrefab { get; set; }

        [field: SerializeField,
                InfoBox("Key = Card Data Scriptable Object : Value = Amount of that card type initially")]
        private Dictionary<CardData, int> InitialCards { get; set; }

        [ShowInInspector, ReadOnly] private Queue<Card> _deckCards = new();
        [ShowInInspector, ReadOnly] private Stack<Card> _dealtCards = new();

        private void Awake()
        {
            // PopulateDeckWithInitialCards();
        }

        private void PopulateDeckWithInitialCards()
        {
            foreach (var kv in InitialCards)
            {
                for (var i = 0; i < kv.Value; i++)
                {
                    _deckCards.Enqueue(new Card()
                    {
                        Data = kv.Key
                    });
                }
            }
        }
    }
}