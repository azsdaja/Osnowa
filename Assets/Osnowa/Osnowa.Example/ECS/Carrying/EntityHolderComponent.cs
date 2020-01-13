using System;
using Entitas;
using UnityEngine;

namespace Osnowa.Osnowa.Example.ECS.Carrying
{
	[Serializable]
	public class EntityHolderComponent : IComponent
	{
		[SerializeField]
		private Guid _entityHeld;

		public Guid EntityHeld
		{
			get { return _entityHeld; }
			set { _entityHeld = value; }
		}
	}
}