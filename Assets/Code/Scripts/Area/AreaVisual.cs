using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AreaVisual : MonoBehaviour
    {
        [field: SerializeField]
        private Area Area { get; set; }

        [field: SerializeField, ChildGameObjectsOnly]
        private GameObject HiddenArea { get; set; }

        [field: SerializeField, ChildGameObjectsOnly]
        private GameObject RevealedArea { get; set; }

        public void CheckIfShouldBeRevealed()
        {
            if (!Area.Revealed)
                return;

            HiddenArea.SetActive(false);
            RevealedArea.SetActive(true);
        }
    }
}