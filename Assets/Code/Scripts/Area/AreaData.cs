using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Area Data", fileName = "Area Data")]
    public class AreaData : SerializedScriptableObject
    {
        public struct CardDataSpawnChance
        {
            [AssetSelector, HorizontalGroup, HideLabel]
            public CardData card;

            [HorizontalGroup, LabelWidth(50), Tooltip("Weight Drop Chance Value")]
            public float weight;

            [HorizontalGroup, LabelText("Guaranteed"), LabelWidth(100),
             Tooltip("Guarantee at least one of these cards")]
            public bool guaranteedOne;
        }

        [field: SerializeField, Tooltip("Valid Card Data that can be the root card."), InlineProperty]
        public List<CardDataSpawnChance> PossibleCardSpawnsInArea { get; private set; } = new();

        [field: SerializeField, Tooltip("How many root cards can be belong to this Area.")]
        public int MaxCardCount { get; set; } = 4;

        private float MaxWeight => PossibleCardSpawnsInArea.Sum(c => c.weight);

        public List<CardData> GetSpawnedCards(bool fillEmptySlotsWithEmptyPlots = true)
        {
            var guaranteedCards = PossibleCardSpawnsInArea.Where(s => s.guaranteedOne).ToList();
            var spawnedCards = guaranteedCards.Select(guaranteed => guaranteed.card).ToList();
            var leftOverCardSpace = MaxCardCount - spawnedCards.Count;

            for (var i = 0; i < leftOverCardSpace; i++)
            {
                var randomCard = GetRandomCardSpawn();
                if (randomCard == null && fillEmptySlotsWithEmptyPlots)
                    spawnedCards.Add(CardManager.Instance.EmptyPlotCard);

                if (randomCard != null)
                    spawnedCards.Add(randomCard);
            }

            return spawnedCards;
        }

        private CardData GetRandomCardSpawn()
        {
            var random = Random.Range(0, MaxWeight);

            for (var i = 0; i < PossibleCardSpawnsInArea.Count; i++)
            {
                var cardChance = PossibleCardSpawnsInArea.ElementAt(i);
                random -= cardChance.weight;
                if (random < 0)
                    return cardChance.card;
            }

            return null;
        }
    }
}