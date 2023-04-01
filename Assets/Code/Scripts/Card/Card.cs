using System;
using Sirenix.OdinInspector;

namespace SolitaireSettlement
{
    [Serializable]
    public class Card
    {
        [field: ShowInInspector, ReadOnly]
        public CardData Data { get; set; }
    }
}