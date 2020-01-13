using Osnowa.Osnowa.Core;
using Osnowa.Osnowa.Entities;

public class PositionedEntity : IPositionedEntity
{
    private readonly GameEntity _gameEntity;

    public PositionedEntity(GameEntity gameEntity)
    {
        _gameEntity = gameEntity;
    }

    public Position Position => _gameEntity.position.Position;

    public void Show()
    {
        if (_gameEntity.hasView)
            _gameEntity.view.Controller.Show();
    }

    public void Hide()
    {
        if (_gameEntity.hasView)
            _gameEntity.view.Controller.Hide();
    }
}