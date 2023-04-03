using UnityEngine;

namespace SolitaireSettlement
{
    public interface IUIDraggable
    {
        public bool CanBeDragged { get; set; }

        void OnDrag(Vector2 position);
        void OnDragCancel();
    }
}