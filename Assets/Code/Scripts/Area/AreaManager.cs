using Simplicity.GameEvent;
using Simplicity.Singleton;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AreaManager : Singleton<AreaManager>
    {
        [field: SerializeField]
        private GameEvent OnAreaRevealedEvent { get; set; }

        
    }
}