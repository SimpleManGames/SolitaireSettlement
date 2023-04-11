using System;
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
            Stages.ForEach(s => s.Setup());

            _currentIndex = _currentIndex > Stages.Count ? 0 : _currentIndex + 1;
            _currentStage = Stages.ElementAt(_currentIndex);
            _currentStage.ExecuteStageLogic();
        }

        private void Update()
        {
            if (!_currentStage.HasFinished())
                return;

            _currentIndex = _currentIndex + 1 > Stages.Count - 1 ? 0 : _currentIndex + 1;
            try
            {
                _currentStage = Stages.ElementAt(_currentIndex);
                _currentStage.ExecuteStageLogic();
                Debug.Log($"Current Stage Now:{_currentStage}");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogError(
                    $"Game Stage Manager - New Index was invalid! Current Index:{_currentIndex}\n{e.Message}");
            }
        }
    }
}