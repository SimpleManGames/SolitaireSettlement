using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SolitaireSettlement
{
    public class CardRenderer : MonoBehaviour
    {
        [field: Title("Palette")]
        [field: SerializeField]
        public CardPaletteData Palette { get; set; }

        [field: Title("UI Objects - Images")]
        [field: SerializeField, ChildGameObjectsOnly, Required]
        private Image BackgroundImage { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private Image NameBackgroundImage { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private Image ArtBackgroundImage { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private Image BorderImage { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private Image NameUnderlineImage { get; set; }

        [field: Title("UI Objects - Text")]
        [field: SerializeField, ChildGameObjectsOnly, Required]
        private TextMeshProUGUI NameText { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private TextMeshProUGUI HungerText { get; set; }

        public void UpdateCardVisuals(CardData data)
        {
            Palette = data.ColorPalette;
            NameText.text = data.Name;
            gameObject.name = data.Name;

            UpdateDynamicTextFields(data);
            SetVisuals();
        }

        public void UpdateDynamicTextFields(CardData data)
        {
            UpdateHungerText(data);
        }

        private void SetVisuals()
        {
            SetGraphicColor(BackgroundImage, Palette.PrimaryColor);
            SetGraphicColor(NameBackgroundImage, Palette.SecondaryColor);
            SetGraphicColor(ArtBackgroundImage, Palette.SecondaryColor);
            SetGraphicColor(BorderImage, Palette.BorderColor);
            SetGraphicColor(NameUnderlineImage, Palette.BorderColor);

            SetGraphicColor(NameText, Palette.NameColor);
        }

        private void UpdateHungerText(CardData data)
        {
            if (data.CardUse is PersonHungerCardUse hungerCardUse)
                HungerText.text = $"{hungerCardUse.CurrentHunger}/{hungerCardUse.HungerCap}";
            else
                HungerText.text = "";
        }

        private static void SetGraphicColor(Graphic image, Color color)
        {
            if (image != null)
                image.color = color;
        }
    }
}