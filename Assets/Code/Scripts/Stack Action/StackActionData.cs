using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Stack Action Data", fileName = "New Stack Action")]
    public class StackActionData : SerializedScriptableObject
    {
        [field: SerializeField, Delayed]
        private string Name { get; set; }

        [field: SerializeField]
        public List<CardData> NeededCardsInStack { get; private set; }

        [field: SerializeField]
        public IStackActionResult Result { get; private set; }

        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(path, Name + " Stack Action");
        }
    }
}