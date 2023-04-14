using System.Collections.Generic;
using System.Linq;
using Simplicity.Utility;
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

        [field: SerializeField, Required, HideReferenceObjectPicker]
        public IStackActionResult[] Results { get; private set; }

        public bool Conflict => _conflictedStackActions.Count > 0 || Results == null || Results.Length == 0;

        private List<StackActionData> _conflictedStackActions;

        [Title("Conflicts"), ShowInInspector, ReadOnly, ShowIf("@this.ConflictedStackActions.Count > 0")]
        private List<StackActionData> ConflictedStackActions
        {
            get { return _conflictedStackActions = CheckConflicts(this, true); }
        }

        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(path, Name + " Stack Action");
        }

        private static List<StackActionData> CheckConflicts(StackActionData data, bool checkOther = true)
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
                        CheckConflicts(otherStack, false);
                }
            }

            return results;
        }
    }
}