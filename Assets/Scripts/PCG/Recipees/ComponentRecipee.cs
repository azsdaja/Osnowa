namespace PCG.Recipees
{
	using Osnowa.Osnowa.Rng;
	using UnityEngine;

	public abstract class ComponentRecipee : ScriptableObject, IComponentRecipee
	{
		public abstract void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng);
	}
}