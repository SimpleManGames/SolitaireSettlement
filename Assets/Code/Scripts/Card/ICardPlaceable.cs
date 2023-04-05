namespace SolitaireSettlement
{
    /// <summary>
    /// Interface determining if a card can be placed on an object.
    /// </summary>
    public interface ICardPlaceable
    {
        bool IsValidPlacement(ICardPlaceable placeable);
    }
}