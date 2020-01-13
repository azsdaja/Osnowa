namespace GameLogic.AI.SkillEvaluation
{
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.RNG;

	class SkillEvaluationContext : ISkillEvaluationContext
	{
		public SkillEvaluationContext(IRandomNumberGenerator rng, IEntityDetector entityDetector, IExampleContextManager contextManager)
		{
			Rng = rng;
			EntityDetector = entityDetector;
			ContextManager = contextManager;
		}

		public IRandomNumberGenerator Rng { get; }
		public IEntityDetector EntityDetector { get; }
		public IExampleContextManager ContextManager { get; }
	}
}