using System.Collections.Generic;
using System.Linq;
using Simplicity.Utility;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Stack Action Data", fileName = "New Stack Action")]
    public class StackActionData : SerializedScriptableObject
    {
        [field: SerializeField, Delayed]
        private string Name { get; set; }

        [field: SerializeField, AssetSelector(DropdownTitle = "Select Card Data", IsUniqueList = false),
                ListDrawerSettings(Expanded = true, OnBeginListElementGUI = "NeededCardsInStackListBeginGUI",
                    OnEndListElementGUI = "NeededCardsInStackListEndGUI")]
        public List<CardData> NeededCardsInStack { get; private set; }

        [field: SerializeField, Required, HideReferenceObjectPicker]
        public IStackActionResult[] Results { get; private set; }

        public bool Conflict => ConflictedStackActions?.Count > 0 || Results?.Length == 0;

        private int _lastAmountOfNeededCards = 0;

        [Title("Conflicts"), ShowInInspector, ShowIf("@this.ConflictedStackActions.Count > 0"),
         InlineButton("CheckConflicts", Label = "", Icon = SdfIconType.ArrowRepeat),
         ListDrawerSettings(Expanded = true, DraggableItems = false, HideAddButton = true, HideRemoveButton = true,
             IsReadOnly = true)]
        private List<StackActionData> ConflictedStackActions { get; set; }

        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(path, Name + " Stack Action");

            CheckConflicts();
        }

        private static List<StackActionData> GetConflicts(StackActionData data, bool checkOther = true)
        {
            var results = new List<StackActionData>();
            var otherStackActions = AssetParsingUtility.FindAssetsByType<StackActionData, List<StackActionData>>()
                .Where(s => data != s).ToList();

            foreach (var otherStack in otherStackActions)
            {
                if (!StackActionManager.CheckForSimilarCards(otherStack, data.NeededCardsInStack))
                    continue;

                if (StackActionManager.CheckForFullMatching(otherStack, data.NeededCardsInStack))
                {
                    results.Add(otherStack);
                    if (checkOther)
                        otherStack.CheckConflicts(true);
                }
            }

            return results;
        }

        private void CheckConflicts(bool calledFromOther = false)
        {
            _lastAmountOfNeededCards = -1;

            if (_lastAmountOfNeededCards != NeededCardsInStack.Count)
            {
                _lastAmountOfNeededCards = NeededCardsInStack.Count;
                ConflictedStackActions = GetConflicts(this, !calledFromOther);
            }
        }

        private void NeededCardsInStackListBeginGUI(int index)
        {
        }

        private void NeededCardsInStackListEndGUI(int index)
        {
            var buttonCardData = NeededCardsInStack.ElementAt(index);
            foreach (var needed in NeededCardsInStack.Except(new[] { buttonCardData }))
            {
                if (buttonCardData.ValidStackableCards.Contains(needed))
                    continue;

                SirenixEditorGUI.WarningMessageBox(
                    $"{buttonCardData.Name}'s Valid Cards doesn't contain {needed.Name}");
                if (!SirenixEditorGUI.IconButton(EditorIcons.Plus))
                    continue;

                if (!buttonCardData.ValidStackableCards.Contains(needed))
                    buttonCardData.ValidStackableCards.Add(needed);
            }
        }
    }
}