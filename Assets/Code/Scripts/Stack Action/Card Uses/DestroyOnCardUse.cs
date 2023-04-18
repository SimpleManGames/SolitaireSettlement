using UnityEngine;

namespace SolitaireSettlement
{
    public class DestroyOnCardUse : IStackActionCardUse
    {
        public void Initialize()
        {
            
        }

        public void OnCardUse(Card cardObject)
        {
            CardManager.Instance.RequestToDeleteCardObject(cardObject.gameObject);
        }
    }
}