using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Stack Action Data", fileName = "New Stack Action")]
    public class StackActionData : SerializedScriptableObject
    {
        [field: SerializeField]
        public List<CardData> NeededCardsInStack { get; private set; }

        [field: SerializeField]
        public IStackActionResult Result { get; private set; }
    }
}