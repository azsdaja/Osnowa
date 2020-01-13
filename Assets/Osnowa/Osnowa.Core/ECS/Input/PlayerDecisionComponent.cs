using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Osnowa.Osnowa.Core.ECS.Input
{
	using GameLogic.GameCore;

	[Unique]
	public class PlayerDecisionComponent : IComponent
	{
		public Decision Decision { get; set; }
		public Position Direction { get; set; }
		public Position Position { get; set; }
	}
}