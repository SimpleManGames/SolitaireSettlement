using UnityEngine;

namespace SolitaireSettlement
{
    public interface IUIDrag
    {
        public bool CanBeDragged { get; set; }
        public bool IsBeingDragged { get; set; }

        void OnDragStart(Vector2 position);
        void OnDrag(Vector2 position);
        void OnDragEnd();
    }
}