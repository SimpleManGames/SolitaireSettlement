using System.Collections.Generic;
using Simplicity.GameEvent;
using Simplicity.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SolitaireSettlement
{
    public class GameAreaInteractionsManager : MonoBehaviour, IInteractionsManager
    {
        [field: Title("References")]
        [field: SerializeField] private Camera GameCamera { get; set; }

        [field: SerializeField] private Canvas GameAreaCanvas { get; set; }

        [field: Title("Events")]
        [field: SerializeField] private GameEvent OnCardInteractedEvent { get; set; }

        [field: Title("Settings")]
        [field: SerializeField] private LayerMask InteractableLayerMask { get; set; }

        private InputInteractionsManager InputInteractions { get; set; }

        public GraphicRaycaster UIRaycaster { get; private set; }

        public PointerEventData ClickData { get; private set; }

        public List<RaycastResult> ClickResults { get; private set; }

        private void Awake()
        {
            InputInteractions = GetComponent<InputInteractionsManager>();

            UIRaycaster = GameAreaCanvas.GetComponent<GraphicRaycaster>();
            ClickData = new PointerEventData(EventSystem.current);
            ClickResults = new List<RaycastResult>();
        }

        private void Update()
        {
            if (InputInteractions.CurrentDragObject == null || InputInteractions.CurrentDraggable == null)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)GameAreaCanvas.transform, InputInteractions.InteractionPoint, GameCamera,
                out var position);
            InputInteractions.CurrentDraggable.OnDrag(position);
        }

        public void CastRaysToUI()
        {
            ClickData.position = InputInteractions.InteractionPoint;
            ClickResults.Clear();

            UIRaycaster.Raycast(ClickData, ClickResults);
        }

        public void HandleInteractionResults()
        {
            HandleCardStackInteraction();
            OnCardInteractedEvent.Raise();
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

        private void HandleCardStackInteraction()
        {
            if (InputInteractions.CurrentDragObject == null)
                return;

            LoopThroughAllCardObjects();
        }

        private void LoopThroughAllCardObjects()
        {
            var rectTransform = InputInteractions.CurrentDragObject.GetComponent<RectTransform>();
            var placeables = GameObject.FindGameObjectsWithTag("Card");
            foreach (var placeable in placeables)
            {
                var placeableComponent = placeable.GetComponent(typeof(ICardPlaceable)) as ICardPlaceable;

                if (placeableComponent == null)
                    continue;

                if (placeable == InputInteractions.CurrentDragObject)
                    continue;

                var otherRectTransform = placeable.GetComponent<RectTransform>();
                if (!rectTransform.Overlaps(otherRectTransform))
                    continue;

                if (placeableComponent.OnPlaced(otherRectTransform.gameObject,
                        InputInteractions.CurrentDragObject.gameObject))
                    return;
            }

            UpdateStackInfoForDragObject(InputInteractions.CurrentDragObject);
        }

        private void UpdateStackInfoForDragObject(GameObject dragObject)
        {
            var cardComponent = dragObject.GetComponent<Card>();
            if (cardComponent.IsInStack)
            {
                if (cardComponent.IsOnTop)
                {
                    cardComponent.Stack.RemoveCard(cardComponent);
                }
                else if (!cardComponent.IsOnBottom)
                {
                    var cards = cardComponent.Stack.SplitAt(cardComponent);
                    cardComponent.Stack.RemoveCards(cards);
                    cardComponent.Stack = new CardStack();
                    cardComponent.Stack.AddCards(cards);
                }
            }
        }
    }
}