using System.Collections.Generic;
using System.Linq;
using Simplicity.Utility;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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

        public bool Conflict { get; private set; }

        [Title("Conflicts"), ShowInInspector, ReadOnly, ShowIf("@this.Conflict")]
        private List<StackActionData> _conflictedStackActions = new();

        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(path, Name + " Stack Action");

            foreach (var conflicts in _conflictedStackActions)
                CheckConflicts(conflicts, false);

            CheckConflicts(this);
        }

        private static void CheckConflicts(StackActionData data, bool checkOther = true)
        {
            data._conflictedStackActions.Clear();
            var otherStackActions = AssetParsingUtility.FindAssetsByType<StackActionData>()
                .Where(s => data != s).ToList();

            foreach (var otherStack in otherStackActions)
            {
                if (!StackActionManager.CheckForSimilarCards(otherStack, data.NeededCardsInStack))
                    continue;

                if (StackActionManager.CheckForFullMatching(otherStack, data.NeededCardsInStack))
                {
                    data._conflictedStackActions.Add(otherStack);
                    if (checkOther)
                        CheckConflicts(otherStack, false);

                    data.Conflict = true;
                    return;
                }
            }

            data.Conflict = false;
        }
    }
}