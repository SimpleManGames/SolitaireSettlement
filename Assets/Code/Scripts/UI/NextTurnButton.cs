using Simplicity.GameEvent;
using UnityEngine;

namespace SolitaireSettlement
{
    public class NextTurnButton : MonoBehaviour
    {
        [field: SerializeField]
        private GameEvent OnNextTurnButtonClicked { get; set; }

        public void RaiseOnNextTurnButtonClickedEvent()
        {
            OnNextTurnButtonClicked.Raise();
        }
    }
}