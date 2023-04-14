using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class ChanceAddCardResult : AddCardResult
    {
        protected struct CardChance
        {
            [field: SerializeField, HorizontalGroup("Info"), Required]
            public CardData ProducedCard { get; set; }

            [field: SerializeField, HorizontalGroup("Info", 0.2f), Range(0.0f, 1.0f), HideLabel]
            public float Chance { get; set; }
        }

        protected bool ProcessCardChances(CardChance card, Vector3 spawnPosition, float animateDelay = 0.0f)
        {
            if (!(Random.value <= card.Chance))
                return false;

            CreateCardToDeck(card.ProducedCard, spawnPosition, animateDelay);
            return true;
        }
    }
}