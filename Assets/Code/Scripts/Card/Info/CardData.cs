using System.Collections.Generic;
using System.Linq;
using Simplicity.Utility;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

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

        [ShowInInspector, HorizontalGroup("References", Title = "References", MaxWidth = 0.5f), ReadOnly]
        private List<StackActionData> CreatedByStackAction => CreatedByStackActions();

        [ShowInInspector, HorizontalGroup("References", MaxWidth = 0.5f), ReadOnly]
        private List<StackActionData> UsedInStackAction => UsedInStackActions();

        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(path, Name + " Card Data");
        }

        private List<StackActionData> CreatedByStackActions()
        {
            var results = new List<StackActionData>();
            var addCardResults = AssetParsingUtility.FindAssetsByType<StackActionData>()
                .Where(s => s.Result != null).Select(data => (data, data.Result)).ToList();

            foreach (var kv in addCardResults)
            {
                if (kv.Result is not AddCardResult cast)
                    continue;

                if (cast.AddedCardData().Any(c => c.Name == Name))
                {
                    results.Add(kv.data);
                    break;
                }
            }

            return results;
        }

        private List<StackActionData> UsedInStackActions()
        {
            return AssetParsingUtility.FindAssetsByType<StackActionData>()
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