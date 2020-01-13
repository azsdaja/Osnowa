using System;
using Entitas;
using UnityEngine;

namespace Osnowa.Osnowa.Example.ECS.View
{
	[Serializable]
	public class LooksComponent : IComponent
	{
		public Sprite BodySprite;
	}
}