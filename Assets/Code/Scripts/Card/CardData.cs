using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [CreateAssetMenu(menuName = "Solitaire Settlement/Card/Card Data", fileName = "New Card")]
    public class CardData : ScriptableObject
    {
        public enum ECardType
        {
            Person,
            Resource,
            Building,
            Gathering
        }

        [field: Title("Visuals")]
        [field: SerializeField] 
        public string Name { get; private set; }

        [field: SerializeField, AssetsOnly, Required]
        public CardPaletteData ColorPalette { get; private set; }

        [field: Title("Settings")]
        [field: SerializeField] public ECardType CardType { get; private set; }
    }
}