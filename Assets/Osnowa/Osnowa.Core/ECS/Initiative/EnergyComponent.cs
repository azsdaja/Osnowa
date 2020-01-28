using System;
using Entitas;
using UnityEngine;

namespace Osnowa.Osnowa.Core.ECS.Initiative
{
	[Game, Serializable]
	public class EnergyComponent : IComponent
	{
		/// <summary>
		/// An actor regains energy with rate equal to this value (typically 0.1) with each cycle that goes over all the actors.
		/// </summary>
		public float EnergyGainPerSegment;

		/// <summary>
		/// Amount of energy for performing any actions. By default a player is given control only when his energy
		/// reaches 1. Each action costs energy to perform, by default 1.
		/// </summary>
		public float Energy;
	}
}