using System;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SolitaireSettlement
{
    [Serializable]
    public class CardRuntimeInfo
    {
        [field: ShowInInspector, ReadOnly, InlineEditor]
        public CardData Data { get; protected internal set; }

        public enum CardLocation
        {
            GameBoard,
            Deck,
            Hand,
            Discard,
            Delete
        }

        [field: SerializeField]
        public CardLocation Location { get; private set; }

        [field: ShowInInspector, ReadOnly]
        public GameObject RelatedGameObject { get; private set; }

        [field: SerializeField]
        public Vector3 Position { get; private set; }

        [field: Title("Screen Space Elements Settings")]
        [field: SerializeField]
        public Vector3 ScreenSpaceElementScale { get; private set; } = new Vector3(15, 15, 1);

        [field: Title("GameBoard Settings")]
        [field: SerializeField]
        public Vector3 GameBoardScale { get; private set; } = new Vector3(1, 1, 1);

        public CardRuntimeInfo(CardData data, CardLocation location, bool animate = false)
        {
            Data = data;
            SetCardLocation(location, animate);
        }

        public void SetCardLocation(CardLocation location, bool animate = false)
        {
            Location = location;
            switch (Location)
            {
                case CardLocation.GameBoard:
                    SetGameObjectGameBoardSettings();
                    break;
                case CardLocation.Deck:
                    SetGameObjectDeckSettings(animate);
                    break;
                case CardLocation.Hand:
                    SetGameObjectInHandSettings(animate);
                    break;
                case CardLocation.Discard:
                    SetGameObjectDiscardSettings(animate);
                    break;
                case CardLocation.Delete:
                    RelatedGameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetRelatedGameObject(GameObject gameObject)
        {
            RelatedGameObject = gameObject;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }

        private void SetGameObjectGameBoardSettings()
        {
            RelatedGameObject.transform.SetParent(CardManager.Instance.GameAreaCanvas.transform);
            RelatedGameObject.transform.localScale = GameBoardScale;
            RelatedGameObject.transform.rotation = CardManager.Instance.GameAreaCanvas.transform.rotation;
        }

        private void SetGameObjectDeckSettings(bool animate)
        {
            if (animate && RelatedGameObject == null)
                SetRelatedGameObject(CardFactory.Instance.CreateCardObjectFromDataInUICanvas(Data, this));

            Action onComplete = () =>
            {
                RelatedGameObject.transform.position = Position = DeckManager.Instance.DeckPosition;
            };

            if (!animate || RelatedGameObject == null)
            {
                onComplete.Invoke();
                return;
            }

            SetGameObjectToScreenSpace();

            SetCardAnimationValues(Position, DeckManager.Instance.DeckPosition, onComplete);
        }

        private void SetGameObjectInHandSettings(bool animate)
        {
            if (RelatedGameObject == null)
                SetRelatedGameObject(CardFactory.Instance.CreateCardObjectFromDataInUICanvas(Data, this));

            RelatedGameObject.SetActive(true);

            SetGameObjectToScreenSpace();

            Action onComplete = () =>
            {
                Position = RelatedGameObject.transform.position = HandManager.Instance.DrawnCardTargetPosition;
                RelatedGameObject.transform.SetParent(HandManager.Instance.HandContainer.transform);
            };

            if (!animate)
            {
                onComplete.Invoke();
                return;
            }

            SetCardAnimationValues(RelatedGameObject.transform.position,
                HandManager.Instance.DrawnCardTargetPosition,
                onComplete);
        }

        private void SetGameObjectDiscardSettings(bool animate)
        {
            if (RelatedGameObject == null)
                return;

            if (!animate)
                return;

            SetGameObjectToScreenSpace();
            SetCardAnimationValues(Position, DiscardManager.Instance.DiscardCardPosition,
                () =>
                {
                    RelatedGameObject.SetActive(false);
                    //Object.Destroy(RelatedGameObject);
                });
        }

        private void SetGameObjectToScreenSpace()
        {
            if (RelatedGameObject == null)
                return;

            var cardTransform = RelatedGameObject.transform;

            cardTransform.SetParent(CardManager.Instance.ScreenSpaceCanvas.transform);

            cardTransform.position = Position;
            cardTransform.rotation = Quaternion.identity;
            cardTransform.localScale = ScreenSpaceElementScale;
        }

        private void SetCardAnimationValues(Vector3 from, Vector3 target, Action onComplete)
        {
            var cardAnimator = RelatedGameObject.GetComponent<CardAnimator>();
            cardAnimator.AnimateTo(from, target, onComplete);
        }
    }
}