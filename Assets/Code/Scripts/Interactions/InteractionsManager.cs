using System.Collections.Generic;
using System.Linq;
using Simplicity.GameEvent;
using Simplicity.UI;
using Sirenix.OdinInspector;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SolitaireSettlement
{
    public class InteractionsManager : MonoBehaviour, GameInputs.IInteractionsActions
    {
        [field: Title("References")]
        [field: SerializeField] private Camera GameCamera { get; set; }

        [field: SerializeField] private Canvas InteractableUICanvas { get; set; }

        [field: Title("Events")]
        [field: SerializeField] private GameEvent OnCardInteractedEvent { get; set; }

        [field: Title("Settings")]
        [field: SerializeField] private LayerMask InteractableLayerMask { get; set; }

        [field: Title("Readonly")]
        [field: SerializeField, ReadOnly]
        private Vector2 InteractionPoint { get; set; }

        [field: SerializeField, ReadOnly]
        private Vector3 InteractionWorldPoint { get; set; }

        private GameInputs _gameInputs;

        private RaycastHit[] _clickResults;

        private Vector3 _dragOffset;

        [ShowInInspector]
        private GameObject _currentDragObject;

        [ShowInInspector]
        private IUIDrag _currentDraggable;

        [ShowInInspector]
        private GameObject _currentClickObject;

        [ShowInInspector]
        private IUIClickable _currentClickable;

        private void Awake()
        {
            _gameInputs = new GameInputs();
            _gameInputs.Interactions.SetCallbacks(this);
            _gameInputs.Enable();

            _clickResults = new RaycastHit [10];
        }

        private void Update()
        {
            if (_currentDragObject == null || _currentDraggable == null)
                return;

            var interactionPointVec3 = (Vector3)InteractionPoint;
            interactionPointVec3.z = _currentDragObject.transform.position.z - GameCamera.transform.position.z;
            _currentDraggable.OnDrag(GameCamera.ScreenToWorldPoint(interactionPointVec3));
            _currentDragObject.transform.position = _currentDragObject.transform.position - Vector3.forward;
        }

        public void OnInteractionPoint(InputAction.CallbackContext context)
        {
            InteractionPoint = context.ReadValue<Vector2>();
        }

        public void OnPress(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    var ray = GameCamera.ScreenPointToRay(InteractionPoint);
                    Debug.DrawRay(ray.origin, ray.direction * 200.0f, Color.blue, 10.0f);
                    Physics.RaycastNonAlloc(ray, _clickResults, 200.0f);

                    if (ParseClickResultsForClickable())
                        return;

                    if (ParseClickResultsForDraggable())
                        return;

                    break;
                case InputActionPhase.Canceled:
                    HandleCardStackInteraction();
                    OnCardInteractedEvent.Raise();

                    _currentClickable?.OnClick();
                    _currentDraggable?.OnDragEnd();

                    _currentDragObject = null;
                    _currentDraggable = null;
                    _currentClickObject = null;
                    _currentClickable = null;
                    break;
                case InputActionPhase.Disabled:
                case InputActionPhase.Waiting:
                case InputActionPhase.Started:
                default:
                    break;
            }
        }

        private void HandleCardStackInteraction()
        {
            if (_currentDragObject == null)
                return;

            LoopThroughAllCardObjects();
        }

        private void LoopThroughAllCardObjects()
        {
            var currentDragCollider = _currentDragObject.GetComponent<BoxCollider>();
            foreach (var card in GameObject.FindGameObjectsWithTag("Card"))
            {
                if (card == _currentDragObject)
                    continue;

                var otherCollider = card.GetComponent<BoxCollider>();
                if (!currentDragCollider.bounds.Intersects(otherCollider.bounds))
                    continue;

                Debug.Log($"Intersecting between {currentDragCollider.name}:{otherCollider.name}");

                var placeOntoCard = otherCollider.GetComponent<Card>();
                var draggingCard = _currentDragObject.GetComponent<Card>();

                if (placeOntoCard.Stack != null && placeOntoCard.Stack.Cards.Contains(draggingCard))
                    continue;

                if (!placeOntoCard.IsValidPlacement(draggingCard))
                    break;

                UpdateStackInfoForDragObject();
                DetermineStackInteractions(placeOntoCard, draggingCard);

                if (_currentDragObject.GetComponent<Card>().IsInDeck)
                    DeckManager.Instance.MoveCardTopCardToGame();

                return;
            }

            if (_currentDragObject.GetComponent<Card>().Data.CardType == CardData.ECardType.Person)
                UpdateStackInfoForDragObject();
        }

        private void UpdateStackInfoForDragObject()
        {
            var cardComponent = _currentDragObject.GetComponent<Card>();
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

        private void DetermineStackInteractions(Card placeOntoCard, Card draggingCard)
        {
            if (placeOntoCard.Stack == null) // New Stack
            {
                // New stack is being made, so we need to add both of them to it
                placeOntoCard.Stack = new CardStack();
                placeOntoCard.Stack.AddCard(placeOntoCard);
            }

            if (draggingCard.Stack != null && draggingCard.Stack.HasCards)
                AddDraggingCardsStackCards(placeOntoCard, draggingCard);
            else
                AddSingleCardToStack(placeOntoCard, draggingCard);
        }

        private static void AddSingleCardToStack(Card placeOntoCard, Card draggingCard)
        {
            placeOntoCard.Stack.AddCard(draggingCard);
        }

        private void AddDraggingCardsStackCards(Card placeOntoCard, Card draggingCard)
        {
            placeOntoCard.Stack.AddCards(draggingCard.Stack.Cards);
        }

        private bool ParseClickResultsForClickable()
        {
            foreach (var result in _clickResults)
            {
                if (result.transform == null)
                    continue;

                var resultGameObject = result.transform.gameObject;
                if (resultGameObject.GetComponent<IUIClickable>() == null)
                    continue;

                _currentClickObject = resultGameObject;
                _currentClickable = resultGameObject.GetComponent<IUIClickable>();
                return true;
            }

            return false;
        }

        private bool ParseClickResultsForDraggable()
        {
            foreach (var result in _clickResults)
            {
                if (result.transform == null)
                    continue;

                var resultGameObject = result.transform.gameObject;
                if (resultGameObject.GetComponent<IUIDrag>() == null)
                    continue;

                _currentDragObject = resultGameObject;
                _currentDraggable = resultGameObject.GetComponent<IUIDrag>();
                _currentDraggable.OnDragStart();

                _dragOffset = _currentDragObject.transform.position - InteractionWorldPoint;
                return true;
            }

            return false;
        }
    }
}