using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AreaFoodProductionCardBonus : AreaProductionCardBonus
    {
        [field: SerializeField, LabelWidth(250)]
        public int BonusToFoodProductionWithinArea { get; private set; }
    }
}