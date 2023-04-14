using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SolitaireSettlement
{
    public class ChanceToAddCardResult : ChanceAddCardResult
    {
        [field: SerializeField]
        private CardChance CardChances { get; set; }

        public override void OnResult(IEnumerable<Card> relatedCardStack)
        {
            ProcessCardChances(CardChances, relatedCardStack.ElementAt(0).transform.position);
        }
    }
}