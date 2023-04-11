using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class GameStageManager : SerializedMonoBehaviour
    {
        [field: SerializeField]
        private List<IGameStage> Stages { get; set; }

        private int _currentIndex = 0;

        [ShowInInspector, ReadOnly]
        private IGameStage _currentStage;

        private void Start()
        {
            _currentIndex = _currentIndex > Stages.Count ? 0 : _currentIndex + 1;
            _currentStage = Stages.ElementAt(_currentIndex);
            _currentStage.ExecuteStageLogic();
        }

        private void Update()
        {
            if (!_currentStage.HasFinished())
                return;

            _currentIndex = _currentIndex > Stages.Count ? 0 : _currentIndex + 1;
            _currentStage = Stages.ElementAt(_currentIndex);
            _currentStage.ExecuteStageLogic();
        }
    }
}