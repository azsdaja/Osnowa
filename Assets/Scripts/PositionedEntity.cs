using Osnowa.Osnowa.Core;
using Osnowa.Osnowa.Entities;

public class PositionedEntity : IPositionedEntity
{
    public GameEntity GameEntity { get; }

    public PositionedEntity(GameEntity gameEntity)
    {
        GameEntity = gameEntity;
    }

    public Position Position => GameEntity.position.Position;

    public void Show()
    {
        if (GameEntity.hasView)
            GameEntity.view.Controller.Show();
    }

    public void Hide()
    {
        if (GameEntity.hasView)
            GameEntity.view.Controller.Hide();
    }
}