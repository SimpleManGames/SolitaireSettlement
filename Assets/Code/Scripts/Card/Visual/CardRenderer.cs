using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using Unity.VisualScripting;
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
        private Image ArtImage { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private Image BorderImage { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private Image NameUnderlineImage { get; set; }

        [field: Title("GameObject Containers")]
        [field: SerializeField, ChildGameObjectsOnly, Required]
        private GameObject HealthIndicatorObject { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private GameObject HungerIndicatorObject { get; set; }

        [field: Title("UI Objects - Text")]
        [field: SerializeField, ChildGameObjectsOnly, Required]
        private TextMeshProUGUI NameText { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private TextMeshProUGUI HungerText { get; set; }

        [field: SerializeField, ChildGameObjectsOnly, Required]
        private TextMeshProUGUI HealthText { get; set; }

        public void UpdateCardVisuals(CardData data)
        {
            Palette = data.ColorPalette;
            NameText.text = data.Name;
            gameObject.name = data.Name;

            UpdateDynamicTextFields(data);
            SetVisuals(data);
        }

        public void UpdateDynamicTextFields(CardData data)
        {
            UpdateTextBasedOnCardImpl<HungerCardImpl>(data, () =>
                {
                    HungerIndicatorObject.SetActive(false);
                    HungerText.text = "";
                },
                impl => { HungerText.text = $"{impl.CurrentHunger}"; });

            UpdateTextBasedOnCardImpl<CardHealthImpl>(data, () =>
                {
                    HealthIndicatorObject.SetActive(false);
                    HealthText.text = "";
                },
                impl => { HealthText.text = $"{impl.CurrentHealth}"; });
        }

        private void SetVisuals(CardData data)
        {
            SetGraphicColor(BackgroundImage, Palette.PrimaryColor);
            SetGraphicColor(NameBackgroundImage, Palette.SecondaryColor);
            SetGraphicColor(ArtBackgroundImage, Palette.SecondaryColor);
            SetGraphicColor(ArtImage, Palette.ArtColor);
            SetGraphicColor(BorderImage, Palette.BorderColor);
            SetGraphicColor(NameUnderlineImage, Palette.BorderColor);

            SetGraphicColor(NameText, Palette.NameColor);

            if (data != null && data.CardImage != null)
                SetGraphicArt(ArtImage, data.CardImage);
        }

        private void UpdateTextBasedOnCardImpl<T>(CardData data, Action onNoImpl, Action<T> onImpl)
            where T : ICardUniqueImpl
        {
            if (data.OnTurnUpdate.IsNullOrEmpty())
            {
                onNoImpl.Invoke();
                return;
            }

            if (!data.OnTurnUpdate.Any(w => w is T))
            {
                onNoImpl.Invoke();
                return;
            }

            var cardImpl = data.OnTurnUpdate
                .Where(w => w is T)
                .Cast<T>().First();

            onImpl.Invoke(cardImpl);
        }

        private void OnValidate()
        {
            SetVisuals(null);
        }

        private static void SetGraphicColor(Graphic image, Color color)
        {
            if (image != null)
                image.color = color;
        }

        private static void SetGraphicArt(Image image, Sprite texture)
        {
            if (image != null && texture != null)
                image.sprite = texture;
            else
                image.color = new Color(0, 0, 0, 0);
        }
    }
}