namespace SolitaireSettlement
{
    public class EnoughCardsForDrawCondition : IStageConditionCheck
    {
        public bool CheckCondition()
        {
            return !DeckManager.Instance.HasEnoughCardsForFullDraw;
        }
    }
}