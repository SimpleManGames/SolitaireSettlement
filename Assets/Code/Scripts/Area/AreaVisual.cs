using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SolitaireSettlement
{
    [RequireComponent(typeof(Area))]
    public class AreaVisual : MonoBehaviour
    {
        [field: SerializeField]
        private Area Area { get; set; }

        [field: SerializeField, ChildGameObjectsOnly]
        private GameObject HiddenArea { get; set; }

        [field: SerializeField, ChildGameObjectsOnly]
        private GameObject RevealedArea { get; set; }

        [field: SerializeField, Range(0, 1)]
        private float ColorReductionPercent { get; set; } = 0.1f;

        private void Start()
        {
            HiddenArea.SetActive(true);
            RevealedArea.SetActive(false);

            CheckIfShouldBeRevealed();
        }

        public void CheckIfShouldBeRevealed()
        {
            if (!Area.ShouldRevealAfterPlanning)
                return;

            Area.Revealed = true;
            Area.ShouldRevealAfterPlanning = false;

            HiddenArea.SetActive(false);
            RevealedArea.SetActive(true);

            Area.OnRevealed();
        }

        public void SetAreaDataColor(Color color)
        {
            var percentageColor = new Color(color.r * ColorReductionPercent, color.g * ColorReductionPercent,
                color.b * ColorReductionPercent);

            var hiddenImage = HiddenArea.GetComponent<Image>();
            var reducedColor = color - percentageColor;
            hiddenImage.color = new Color(reducedColor.r, reducedColor.g, reducedColor.b, 1.0f);

            var shownImage = RevealedArea.GetComponent<Image>();
            shownImage.color = color;
        }
    }
}