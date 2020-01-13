namespace Osnowa.Osnowa.Entities
{
    using Core;

    public interface IPositionedEntity
    {
        Position Position { get; }
        void Show();
        void Hide();
    }
}