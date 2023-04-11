namespace SolitaireSettlement
{
    public interface IGameStage
    {
        bool HasFinished();

        void ExecuteStageLogic();
    }
}