namespace UI
{
    using GameLogic.Entities;
    using Osnowa.NaPozniej;

    public interface IEntityUiPresenter
    {
        void SetStatus(ViewStatusClass viewStatusClass, ActorStatusDefinition statusDefinition);
        void RemoveStatus(ViewStatusClass viewStatusClass);
        void SetIntegrityRatio(float integrityRatio);
        void SetProgressRatio(float progress);
    }
}