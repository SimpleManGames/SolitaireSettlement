using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Area Data", fileName = "Area Data")]
    public class AreaData : SerializedScriptableObject
    {
        [field: SerializeField, Tooltip("Valid Card Data that can be the root card.")]
        public HashSet<CardData> ValidCardDataInArea { get; set; } = new();

        [field: SerializeField, Tooltip("How many root cards can be belong to this Area.")]
        public int MaxCardCount { get; set; } = 4;
    }
}