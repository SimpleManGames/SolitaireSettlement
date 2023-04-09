using System;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SolitaireSettlement
{
    [Serializable]
    public class CardFactory : Singleton<CardFactory>
    {
        [field: SerializeField, AssetsOnly]
        private GameObject CardPrefab { get; set; }

        [field: SerializeField]
        private Transform GameCanvasTransform { get; set; }

        [field: SerializeField]
        private Transform UICanvasTransform { get; set; }

        [field: SerializeField]

        private Transform HandCanvasTransform { get; set; }

        public GameObject CreateEmptyCardObject()
        {
            var newCardObject = Object.Instantiate(CardPrefab, GameCanvasTransform);
            return newCardObject;
        }

        public GameObject CreateCardObjectFromData(CardData data)
        {
            var newCardObject = Object.Instantiate(CardPrefab, GameCanvasTransform);
            var newCardComponent = newCardObject.GetComponent<Card>();
            newCardComponent.UpdateCardData(data);
            return newCardObject;
        }

        public GameObject CreateCardObjectFromDataInUICanvas(CardData data)
        {
            var newCardObject = Object.Instantiate(CardPrefab, UICanvasTransform);
            newCardObject.transform.localScale = new Vector3(15, 15, 1);
            var newCardComponent = newCardObject.GetComponent<Card>();
            newCardComponent.UpdateCardData(data);
            return newCardObject;
        }
    }
}