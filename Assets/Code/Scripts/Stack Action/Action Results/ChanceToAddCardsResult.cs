using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class ChanceToAddCardsResult : ChanceAddCardResult
    {
        [field: SerializeField, TableList]
        private List<CardChance> Cards { get; set; }

        public override void OnResult(IEnumerable<Card> relatedCardStack)
        {
            var position = relatedCardStack.ElementAt(0).transform.position;
            var success = 0;
            foreach (var cardChance in Cards)
            {
                if (ProcessCardChances(cardChance, position, success * DELAY_BETWEEN_CARD_SPAWNS))
                    success++;
            }
        }
    }
}