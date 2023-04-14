using System.Collections.Generic;
using System.Linq;
using Simplicity.Utility;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Card/Card Data", fileName = "New Card")]
    public class CardData : SerializedScriptableObject
    {
        public enum ECardType
        {
            Person,
            Resource,
            Building,
            Gathering
        }

        [field: Title("Visuals")]
        [field: SerializeField, Delayed]
        public string Name { get; private set; }

        [field: SerializeField, AssetsOnly, Required]
        public CardPaletteData ColorPalette { get; private set; }

        [field: Title("Settings")]
        [field: SerializeField]
        public ECardType CardType { get; private set; }

        [field: SerializeField]
        public IStackActionCardUse CardUse { get; set; }

        [ShowInInspector, HorizontalGroup("References", Title = "References"), ReadOnly]
        private List<StackActionData> _createdByStackAction = new();

        [ShowInInspector, HorizontalGroup("References"), ReadOnly]
        private List<StackActionData> _usedInStackAction = new();

        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(path, Name + " Card Data");

            _createdByStackAction.Clear();
            _usedInStackAction.Clear();
            var addCardResults = AssetParsingUtility.FindAssetsByType<StackActionData>()
                .Where(s => s.Result != null).Select(data => (data, data.Result)).ToList();

            foreach (var kv in addCardResults)
            {
                if (kv.Result is not AddCardResult cast)
                    continue;

                if (cast.AddedCardData().Any(c => c.Name == Name))
                {
                    _createdByStackAction.Add(kv.data);
                    break;
                }
            }

            _usedInStackAction = AssetParsingUtility.FindAssetsByType<StackActionData>()
                .Where(s => s.NeededCardsInStack.Any(c => c.Name == Name)).ToList();
        }

        public override int GetHashCode()
        {
            unchecked // Allow arithmetic overflow, numbers will just "wrap around"
            {
                var hashcode = 1430287;
                hashcode = hashcode * 7302013 ^ Name.GetHashCode();
                hashcode = hashcode * 7302013 ^ CardType.GetHashCode();
                return hashcode;
            }
        }
    }
}