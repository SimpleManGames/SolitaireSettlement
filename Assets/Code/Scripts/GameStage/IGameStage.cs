namespace SolitaireSettlement
{
    public interface IGameStage
    {
        void Setup();
        
        bool HasFinished();

        void ExecuteStageLogic();
    }
}