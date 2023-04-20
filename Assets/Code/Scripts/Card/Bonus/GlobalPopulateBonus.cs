using UnityEngine;

namespace SolitaireSettlement
{
    public class GlobalPopulateBonus : GlobalCardBonus
    {
        [field: SerializeField]
        public int BonusToPopulateCap { get; private set; }
    }
}