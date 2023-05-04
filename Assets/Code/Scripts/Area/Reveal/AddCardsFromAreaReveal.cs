namespace SolitaireSettlement
{
    public class AddCardsFromAreaReveal : IAreaReveal
    {
        public void OnAreaReveal(Area area)
        {
            CardManager.Instance.AddCardFromAreaReveal(area.CardObjectsInArea);
        }
    }
}