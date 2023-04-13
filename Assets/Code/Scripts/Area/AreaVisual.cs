using Sirenix.OdinInspector;
using UnityEngine;

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
    }
}