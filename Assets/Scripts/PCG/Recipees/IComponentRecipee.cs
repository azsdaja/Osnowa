namespace PCG.Recipees
{
    using Osnowa.Osnowa.RNG;

    public interface IComponentRecipee
    {
        void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng);
    }
}