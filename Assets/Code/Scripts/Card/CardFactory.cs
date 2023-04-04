using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SolitaireSettlement
{
    [Serializable]
    public class CardFactory
    {
        [field: SerializeField, AssetsOnly]
        private GameObject CardPrefab { get; set; }

        [field: SerializeField]
        private Transform CanvasTransform { get; set; }

        public GameObject CreateEmptyCardObject()
        {
            var newCardObject = Object.Instantiate(CardPrefab, CanvasTransform);
            return newCardObject;
        }

        public GameObject CreateCardObjectFromData(CardData data)
        {
            var newCardObject = Object.Instantiate(CardPrefab, CanvasTransform);
            var newCardComponent = newCardObject.GetComponent<Card>();
            newCardComponent.Data = data;
            return newCardObject;
        }
    }
}