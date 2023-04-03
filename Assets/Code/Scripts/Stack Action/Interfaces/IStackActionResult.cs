using System.Collections.Generic;

namespace SolitaireSettlement
{
    public interface IStackActionResult
    {
        void OnResult(IEnumerable<Card> relatedCardStack);
    }
}