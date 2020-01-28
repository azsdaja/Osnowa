namespace PCG.Recipees
{
    using Osnowa.Osnowa.Rng;

    public interface IComponentRecipee
    {
        void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng);
    }
}