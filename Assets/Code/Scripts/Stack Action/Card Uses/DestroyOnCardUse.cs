namespace SolitaireSettlement
{
    public class DestroyOnCardUse : IStackActionCardUse
    {
        public void OnCardUse(Card cardObject)
        {
            CardManager.Instance.RequestToDeleteCardObject(cardObject.gameObject);
        }
    }
}