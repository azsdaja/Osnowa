namespace GameLogic.AI.Model
{
	using System.Collections.Generic;
	using ActivityCreation;
	using SkillEvaluation;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Skill", menuName = "Osnowa/AI/Skills/Skill", order = 0)]
	public class Skill : ScriptableObject
	{
		public SkillEvaluator Evaluator;

		public ActivityCreator ActivityCreator;

		/// <summary>
		/// Stimuli that can trigger execution of this skill by the actor. If there is at least one stimulus, it's required to be present on entity to be applicable.
		/// </summary>
		public List<StimulusType> TriggeringStimuli;

		public List<Condition> Conditions;
	}
}