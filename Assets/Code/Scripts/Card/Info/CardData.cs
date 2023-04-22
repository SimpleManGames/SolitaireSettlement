using System;
using System.Collections.Generic;
using System.Linq;
using Simplicity.Utility;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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
            Food,
            Stockpile
        }

        [field: Title("Visuals")]
        [field: SerializeField, Delayed]
        public string Name { get; private set; }

        [field: SerializeField, AssetsOnly, Required]
        public CardPaletteData ColorPalette { get; private set; }

        [field: Title("Settings")]
        [field: SerializeField]
        public ECardType CardType { get; private set; }

        [field: SerializeField, AssetsOnly, AssetSelector,
                ListDrawerSettings(OnTitleBarGUI = "ValidStackableCardItemSyncAllGUI",
                    OnBeginListElementGUI = "ValidStackableCardElementBeginGUI",
                    OnEndListElementGUI = "ValidStackableCardElementEndGUI", Expanded = true)]
        public List<CardData> ValidStackableCards { get; private set; }

        [field: SerializeField, InlineProperty, HideLabel, Title("On Card Use")]
        public List<IStackActionCardUse> CardUse { get; set; }

        [field: SerializeField, InlineProperty, HideLabel, Title("On Turn Updates")]
        public List<ICardUniqueImpl> OnTurnUpdate { get; set; }

        [field: SerializeField, InlineProperty, HideLabel, Title("Bonuses"), ListDrawerSettings(Expanded = true)]
        public List<ICardBonus> CardBonuses { get; private set; }

        [field: ShowInInspector, HorizontalGroup("References", Title = "References", MaxWidth = 0.5f),
                ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true,
                    DraggableItems = false, IsReadOnly = true)]
        private List<StackActionData> CreatedByStackAction { get; set; }

        [field: ShowInInspector, HorizontalGroup("References", MaxWidth = 0.5f),
                ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true,
                    DraggableItems = false, IsReadOnly = true),
                InlineButton("RefreshReferences", Label = "", Icon = SdfIconType.ArrowRepeat)]
        private List<StackActionData> UsedInStackAction { get; set; }

        public bool Invalid => ValidStackableCards == null || CardUse == null || OnTurnUpdate == null;

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
            var cardResults = AssetParsingUtility.FindAssetsByType<StackActionData, List<StackActionData>>()
                .Where(s => s.Results != null).Select(data => (data, data.Results));

            foreach (var kv in cardResults)
            {
                foreach (var result in kv.Results)
                {
                    if (CheckStackActionResultsForAddCardResult(result, results, kv))
                        continue;

                    if (CheckStackActionResultsForReplaceCardResult(result, results, kv))
                        continue;

                    break;
                }
            }

            return results;
        }

        private bool CheckStackActionResultsForAddCardResult(IStackActionResult result, List<StackActionData> results,
            (StackActionData data, IStackActionResult[] Results) kv)
        {
            if (result is not AddCardResult cast)
                return false;

            if (cast.AddedCardData().IsNullOrEmpty())
                return false;

            var addedCards = cast.AddedCardData();

            if (addedCards.All(c =>
                {
                    try
                    {
                        return c.Name != Name;
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.LogError($"CardData was Null within AddedCardData of {kv.data.name}");
                        throw;
                    }
                }))
                return false;

            results.Add(kv.data);
            return true;
        }

        private bool CheckStackActionResultsForReplaceCardResult(IStackActionResult result,
            List<StackActionData> results, (StackActionData data, IStackActionResult[] Results) kv)
        {
            if (result is not ReplaceCardWithResults cast)
                return false;

            if (cast.ReplacementCard.Name != Name)
                return false;

            results.Add(kv.data);
            return true;
        }

        private List<StackActionData> UsedInStackActions()
        {
            return AssetParsingUtility.FindAssetsByType<StackActionData, List<StackActionData>>()
                .Where(s => s.NeededCardsInStack.Any(c => c.Card.Name == Name)).ToList();
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