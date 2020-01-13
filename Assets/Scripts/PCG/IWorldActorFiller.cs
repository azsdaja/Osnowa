namespace PCG
{
	/// <summary>
	/// Generates new actors (SpecificActorData) and adds them to game context.
	/// </summary>
	public interface IWorldActorFiller
	{
		void FillWithActors(float enemyCountRate = 1f);
	}
}