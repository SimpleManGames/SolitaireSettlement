using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Card/Card Palette Data", fileName = "Card Palette Data")]
    public class CardPaletteData : SerializedScriptableObject
    {
        [field: Title("Colors")]
        [field: SerializeField] public Color PrimaryColor { get; private set; }

        [field: SerializeField] public Color SecondaryColor { get; private set; }

        [field: SerializeField] public Color BorderColor { get; private set; }

        [field: SerializeField] public Color NameColor { get; private set; }

        [field: SerializeField] public Color ArtColor { get; private set; }

        [field: Title("Extras")]
        [field: SerializeField] public Color HeartColor { get; private set; }

        public override int GetHashCode()
        {
            unchecked // Allow arithmetic overflow, numbers will just "wrap around"
            {
                var hashcode = 1430287;
                hashcode = hashcode * 7302013 ^ PrimaryColor.GetHashCode();
                hashcode = hashcode * 7302013 ^ SecondaryColor.GetHashCode();
                hashcode = hashcode * 7302013 ^ BorderColor.GetHashCode();
                hashcode = hashcode * 7302013 ^ NameColor.GetHashCode();
                hashcode = hashcode * 7302013 ^ ArtColor.GetHashCode();
                hashcode = hashcode * 7302013 ^ HeartColor.GetHashCode();
                return hashcode;
            }
        }
    }
}