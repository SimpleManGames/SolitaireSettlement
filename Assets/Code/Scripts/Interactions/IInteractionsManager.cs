using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SolitaireSettlement
{
    public interface IInteractionsManager
    {
        public GraphicRaycaster UIRaycaster { get; }

        public PointerEventData ClickData { get; }

        public List<RaycastResult> ClickResults { get; }

        public void CastRaysToUI();

        public void HandleInteractionResults();
        
        public bool ParseClickResultsForClickable();
        public bool ParseClickResultsForDraggable();
    }
}