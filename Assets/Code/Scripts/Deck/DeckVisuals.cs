using System;
using TMPro;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DeckVisuals : MonoBehaviour
    {
        [field: SerializeField]
        private TextMeshProUGUI Text { get; set; }

        private void Update()
        {
            Text.text = $"{DeckManager.Instance.CardsInDeck}";
        }
    }
}