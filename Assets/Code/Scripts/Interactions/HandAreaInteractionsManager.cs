using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SolitaireSettlement
{
    public class HandAreaInteractionsManager : MonoBehaviour, IInteractionsManager
    {
        [field: Title("References")]
        [field: SerializeField] private Camera GameCamera { get; set; }

        [field: SerializeField] private Canvas HandCanvas { get; set; }

        public GraphicRaycaster UIRaycaster { get; private set; }

        public PointerEventData ClickData { get; private set; }

        public List<RaycastResult> ClickResults { get; private set; }

        private InputInteractionsManager InputInteractions { get; set; }

        private void Awake()
        {
            InputInteractions = GetComponent<InputInteractionsManager>();

            UIRaycaster = HandCanvas.GetComponent<GraphicRaycaster>();
            ClickData = new PointerEventData(EventSystem.current);
            ClickResults = new List<RaycastResult>();
        }

        public void CastRaysToUI()
        {
            ClickData.position = InputInteractions.InteractionPoint;
            ClickResults.Clear();

            UIRaycaster.Raycast(ClickData, ClickResults);
        }

        public void HandleInteractionResults()
        {
        }

        public bool ParseClickResultsForClickable()
        {
            foreach (var result in ClickResults)
            {
                var resultGameObject = result.gameObject;
                if (resultGameObject.GetComponent<IUIClickable>() == null)
                    continue;

                InputInteractions.CurrentClickObject = resultGameObject;
                InputInteractions.CurrentClickable = resultGameObject.GetComponent<IUIClickable>();
                return true;
            }

            return false;
        }

        public bool ParseClickResultsForDraggable()
        {
            foreach (var result in ClickResults)
            {
                var resultGameObject = result.gameObject;
                if (resultGameObject.GetComponent<IUIDrag>() == null)
                    continue;

                InputInteractions.CurrentDragObject = resultGameObject;
                InputInteractions.CurrentDraggable = resultGameObject.GetComponent<IUIDrag>();
                InputInteractions.CurrentDraggable.OnDragStart();
                return true;
            }

            return false;
        }
    }
}