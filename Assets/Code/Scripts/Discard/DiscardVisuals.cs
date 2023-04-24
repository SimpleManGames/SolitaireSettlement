using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DiscardVisuals : MonoBehaviour
    {
        [field: SerializeField, ChildGameObjectsOnly]
        private TextMeshProUGUI AmountText { get; set; }

        private void Update()
        {
            AmountText.text = $"{DiscardManager.Instance.DiscardCardCount}";
        }
    }
}