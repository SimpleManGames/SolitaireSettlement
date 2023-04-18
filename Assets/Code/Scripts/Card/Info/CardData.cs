using System.Collections.Generic;
using System.Linq;
using Simplicity.Utility;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
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
            Gathering,
            Food
        }

        [field: Title("Visuals")]
        [field: SerializeField, Delayed]
        public string Name { get; private set; }

        [field: SerializeField, AssetsOnly, Required]
        public CardPaletteData ColorPalette { get; private set; }

        [field: Title("Settings")]
        [field: SerializeField]
        public ECardType CardType { get; private set; }

        [field: SerializeField, AssetsOnly, AssetSelector(DropdownTitle = "Select Card Data"),
                ListDrawerSettings(OnTitleBarGUI = "ValidStackableCardItemSyncAllGUI",
                    OnBeginListElementGUI = "ValidStackableCardElementBeginGUI",
                    OnEndListElementGUI = "ValidStackableCardElementEndGUI")]
        public List<CardData> ValidStackableCards { get; private set; }

        [field: SerializeField, InlineProperty, HideLabel, Title("On Card Use")]
        public IStackActionCardUse CardUse { get; set; }

        [field: SerializeField, InlineProperty, HideLabel, Title("On Turn Progress")]
        public List<ICardTurnProgress> OnTurnProgress { get; set; }

        [field: ShowInInspector, HorizontalGroup("References", Title = "References", MaxWidth = 0.5f),
                ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true,
                    DraggableItems = false, IsReadOnly = true)]
        private List<StackActionData> CreatedByStackAction { get; set; }

        [field: ShowInInspector, HorizontalGroup("References", MaxWidth = 0.5f),
                ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true,
                    DraggableItems = false, IsReadOnly = true),
                InlineButton("RefreshReferences", Label = "", Icon = SdfIconType.ArrowRepeat)]
        private List<StackActionData> UsedInStackAction { get; set; }

        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(path, Name + " Card Data");

            RefreshReferences();
        }

        private void ValidStackableCardItemSyncAllGUI()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                for (var i = 0; i < ValidStackableCards.Count; i++)
                {
                    var buttonCardData = ValidStackableCards.ElementAt(i);
                    if (!buttonCardData.ValidStackableCards.Contains(this))
                        buttonCardData.ValidStackableCards.Add(this);
                }
            }
        }

        private void ValidStackableCardElementBeginGUI(int index)
        {
        }

        private void ValidStackableCardElementEndGUI(int index)
        {
            var buttonCardData = ValidStackableCards.ElementAt(index);
            if (!buttonCardData.ValidStackableCards.Contains(this))
                SirenixEditorGUI.WarningMessageBox($"{buttonCardData.Name}'s Valid Cards doesn't contain {Name}");
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