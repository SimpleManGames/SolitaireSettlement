using Simplicity.GameEvent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DeckInteract : MonoBehaviour, IUIClickable
    {
        [field: SerializeField, Required, AssetsOnly]
        private GameEvent OnDeckInteractEvent;

        public void OnClick()
        {
            Debug.Log("Deck OnClick");
            OnDeckInteractEvent.Raise();
        }
    }
}