using UnityEngine;

namespace SolitaireSettlement
{
    /// <summary>
    /// Interface determining if a card can be placed on an object.
    /// </summary>
    public interface ICardPlaceable
    {
        bool OnRemoved();
        
        bool OnPlaced(GameObject target, GameObject place);
        
        bool IsValidPlacement(ICardPlaceable placeable);
    }
}