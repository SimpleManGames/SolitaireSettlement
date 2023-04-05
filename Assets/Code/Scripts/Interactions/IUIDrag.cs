using UnityEngine;

namespace SolitaireSettlement
{
    public interface IUIDrag
    {
        public bool CanBeDragged { get; set; }
        public bool IsBeDragging { get; set; }

        void OnDragStart();
        void OnDrag(Vector2 position);
        void OnDragEnd();
    }
}