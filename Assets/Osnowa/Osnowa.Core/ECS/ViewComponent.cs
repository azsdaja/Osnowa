using System;
using Entitas;

namespace Osnowa.Osnowa.Core.ECS
{
	using GameLogic.Entities;

	[Serializable]
	public class ViewComponent : IComponent
	{
		public IViewController Controller;
	}
}