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

        [field: SerializeField, AssetsOnly, AssetSelector(DropdownTitle = "Select Card Data")]
        public List<CardData> ValidStackableCards { get; private set; }

        [field: SerializeField, InlineProperty, HideLabel, Title("On Card Use")]
        public IStackActionCardUse CardUse { get; set; }

        [field: ShowInInspector, HorizontalGroup("References", Title = "References", MaxWidth = 0.5f)]
        private List<StackActionData> CreatedByStackAction { get; set; }

        [field: ShowInInspector, HorizontalGroup("References", MaxWidth = 0.5f),
                InlineButton("RefreshReferences", Label = "", Icon = SdfIconType.ArrowRepeat)]
        private List<StackActionData> UsedInStackAction { get; set; }

        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(path, Name + " Card Data");

            RefreshReferences();
        }

        private void RefreshReferences()
        {
            CreatedByStackAction = CreatedByStackActions();
            UsedInStackAction = UsedInStackActions();
        }

        private List<StackActionData> CreatedByStackActions()
        {
            var results = new List<StackActionData>();
            var addCardResults = AssetParsingUtility.FindAssetsByType<StackActionData, List<StackActionData>>()
                .Where(s => s.Results != null).Select(data => (data, data.Results));

            foreach (var kv in addCardResults)
            {
                foreach (var result in kv.Results)
                {
                    if (result is not AddCardResult cast)
                        continue;

                    if (cast.AddedCardData().All(c => c.Name != Name))
                        continue;

                    results.Add(kv.data);
                    break;
                }
            }

            return results;
        }

        private List<StackActionData> UsedInStackActions()
        {
            return AssetParsingUtility.FindAssetsByType<StackActionData, List<StackActionData>>()
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