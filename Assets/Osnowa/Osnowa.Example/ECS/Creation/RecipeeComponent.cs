using System;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Creation
{
    /// <summary>
    /// Stores the name of a recipee which the entity was created with.
    /// </summary>
	[Serializable]
	public class RecipeeComponent : IComponent
	{
		public string RecipeeName;
	}
}