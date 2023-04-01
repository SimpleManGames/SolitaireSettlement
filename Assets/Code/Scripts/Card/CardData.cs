using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Card/Card Data", fileName = "Card Data")]
    public class CardData : ScriptableObject
    {
        [field: Title("Visuals")]
        [field: SerializeField] private string Name { get; set; }

        [field: SerializeField, AssetsOnly, Required]
        private CardPaletteData ColorPalette { get; set; }
    }
}