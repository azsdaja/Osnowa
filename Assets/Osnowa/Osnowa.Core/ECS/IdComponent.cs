using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Osnowa.Osnowa.Core.ECS
{
	using CSharpUtilities;

	[Game, Serializable]
	public class IdComponent : IComponent
	{
		[PrimaryEntityIndex] public SGuid Id;
	}
}