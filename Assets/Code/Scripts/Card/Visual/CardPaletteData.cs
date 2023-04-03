using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Card/Card Palette Data", fileName = "Card Palette Data")]
    public class CardPaletteData : ScriptableObject
    {
        [field: Title("Colors")]
        [field: SerializeField] public Color PrimaryColor { get; private set; }

        [field: SerializeField] public Color SecondaryColor { get; private set; }

        [field: SerializeField] public Color BorderColor { get; private set; }

        [field: SerializeField] public Color NameColor { get; private set; }

        [field: SerializeField] public Color ArtColor { get; private set; }
    }
}