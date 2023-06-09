using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DeckVisuals : MonoBehaviour
    {
        [field: SerializeField, ChildGameObjectsOnly]
        private TextMeshProUGUI AmountText { get; set; }

        private void Update()
        {
            AmountText.text = $"{DeckManager.Instance.CardsInDeck}";
        }
    }
}