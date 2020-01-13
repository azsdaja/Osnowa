namespace GameLogic.AI.SkillEvaluation
{
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.RNG;

	public interface ISkillEvaluationContext
	{
		IRandomNumberGenerator Rng { get; }
		IEntityDetector EntityDetector { get; }
		IExampleContextManager ContextManager { get; }
	}
}