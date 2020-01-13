namespace GameLogic.AI.Model
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class AiData
	{
		[SerializeField] private List<Need> _needs;
		[SerializeField] private List<Skill> _skills;
		private IDictionary<Need, float> _needSatisfactions;

		public List<Need> Needs
		{
			get { return _needs; }
			set { _needs = value; }
		}

		public List<Skill> Skills
		{
			get { return _skills; }
			set { _skills = value; }
		}

		public IDictionary<Need, float> NeedSatisfactions
		{
			get { return _needSatisfactions; }
			set { _needSatisfactions = value; }
		}
	}
}