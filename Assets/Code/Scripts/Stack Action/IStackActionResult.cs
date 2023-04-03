using System.Collections.Generic;

namespace SolitaireSettlement
{
    public interface IStackActionResult
    {
        void Result(List<Card> relatedCardStack);
    }
}