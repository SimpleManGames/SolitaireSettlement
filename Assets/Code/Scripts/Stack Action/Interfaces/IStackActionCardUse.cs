namespace SolitaireSettlement
{
    public interface IStackActionCardUse
    {
        void Initialize();
        void OnCardUse(Card cardObject);
    }
}