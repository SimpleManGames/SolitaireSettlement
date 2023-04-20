using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public abstract class ChanceAddCardResult : AddCardResult
    {
        public struct CardChance
        {
            [field: SerializeField, HorizontalGroup("Info"), Required, HideLabel]
            public CardData ProducedCard { get; private set; }

            [field: SerializeField, HorizontalGroup("Info", 0.2f), Range(0.0f, 1.0f), HideLabel]
            public float Chance { get; set; }
        }

        protected bool ProcessCardChances(CardChance card, Vector3 spawnPosition, float animateDelay = 0.0f)
        {
            if (!(Random.value <= card.Chance))
                return false;

            CreateCardTo(card.ProducedCard, Location, spawnPosition, animateDelay);
            return true;
        }

        public override List<CardData> AddedCardData()
        {
            throw new System.NotImplementedException();
        }
    }
}