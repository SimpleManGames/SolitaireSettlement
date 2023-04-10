using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [RequireComponent(typeof(AreaVisual))]
    public class Area : MonoBehaviour, ICardPlaceable
    {
        [field: SerializeField]
        private AreaData Data { get; set; }

        private GameObject[] _rootCardObjects;

        [field: ShowInInspector]
        public bool Revealed { get; set; } = false;

        private void Awake()
        {
            _rootCardObjects = new GameObject[Data.MaxCardCount];
            for (var i = 0; i < transform.childCount; i++)
                _rootCardObjects[i] = transform.GetChild(i).gameObject;
        }

        public bool OnPlaced(GameObject target, GameObject place)
        {
            return true;
        }

        public bool IsValidPlacement(ICardPlaceable placeable)
        {
            var cardObject = placeable as Card;
            return !Revealed && cardObject != null && cardObject.Info.Data.CardType == CardData.ECardType.Person;
        }
    }
}