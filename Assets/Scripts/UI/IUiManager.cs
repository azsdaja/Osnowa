namespace UI
{
    using GameLogic;
    using UnityEngine;

    public interface IUiManager : IUiFacade
    {
        Transform UiElementsParent { get; }
    }
}