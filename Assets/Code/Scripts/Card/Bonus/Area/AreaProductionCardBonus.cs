using UnityEngine;

namespace SolitaireSettlement
{
    public abstract class AreaProductionCardBonus : AreaCardBonus
    {
        [field: SerializeField]
        public CardData.ECardType EffectedCardType { get; private set; }
    }
}