using System.Linq;

namespace SolitaireSettlement
{
    public class PersonHungerCardUse : IStackActionCardUse
    {
        public void Initialize()
        {
        }

        public void OnCardUse(Card cardObject)
        {
            var card = cardObject.GetComponent<Card>();

            if (card.Stack.HasCards)
                if (card.Stack.Cards.Any(c => c.Info.Data.CardType == CardData.ECardType.Food))
                    return;

            var hungerImplList = card.Info.Data.OnTurnUpdate.Where(w => w is HungerCardImpl).Cast<HungerCardImpl>().ToList();

            foreach (var hungerImpl in hungerImplList)
            {
                hungerImpl.ModifyHungerBy(-1);
            }
        }
    }
}