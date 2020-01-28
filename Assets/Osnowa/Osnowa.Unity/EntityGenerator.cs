namespace Osnowa.Osnowa.Unity
{
	using System.Collections.Generic;
	using System.Linq;
	using Core;
	using Entitas;
	using GameLogic.AI.Model;
	using PCG.Recipees;
	using Rng;
	using UnityEngine;
	using ILogger = Core.ILogger;

	public class EntityGenerator : IEntityGenerator
	{
		private readonly IRandomNumberGenerator _rng;
		private readonly GameContext _context;
        private readonly ILogger _logger;

        public EntityGenerator(IRandomNumberGenerator rng, GameContext context, ILogger logger)
		{
			_rng = rng;
			_context = context;
            _logger = logger;

        }

		public void GenerateActorFromRecipeeAndAddToContext(IContext<GameEntity> context, 
			IEntityRecipee entityRecipee, Position position, out GameEntity entity, bool controlledByPlayer = false)
		{
			entity = context.CreateEntity();

			entity.AddRecipee(entityRecipee.Id);

			List<IEntityRecipee> allApplyingRecipees = GetAllRecipeesWithBaseFirst(entityRecipee);

            foreach (IEntityRecipee applyingRecipee in allApplyingRecipees)
            {
                if (applyingRecipee.NewComponents.Any(c => c == null))
                    _logger.Warning($"Null component in {applyingRecipee.Name} recipee");
            }
			List<IComponentRecipee> allComponentRecipees = allApplyingRecipees.SelectMany(r => r.NewComponents).ToList();
            foreach (IComponentRecipee recipee in allComponentRecipees)
			{
				recipee.ApplyToEntity(entity, _rng);
			}
			if (entity.hasPosition)
			{
				entity.ReplacePosition(position);
			}

			List<Skill> allSkills = allApplyingRecipees.SelectMany(r => r.NewSkills).ToList();
			if (allSkills.Count > 0)
			{
				entity.AddSkills(allSkills);
			}

			List<Sprite> spritePool = allApplyingRecipees.LastOrDefault(r => r.Sprites != null && r.Sprites.Count > 0).Sprites;
			if (spritePool != null)
			{
				var sprite = _rng.Choice(spritePool);
				entity.ReplaceLooks(sprite);
			}

			if (controlledByPlayer)
			{
				_context.ReplacePlayerEntity(entity.id.Id);
			}

			// duplicated from GameInitializer:
			entity.isFinishedTurn = true;
			if (_context.playerEntity.Id == entity.id.Id)
			{
				entity.isPlayerControlled = true;
			}
		}

		private static List<IEntityRecipee> GetAllRecipeesWithBaseFirst(IEntityRecipee entityRecipee)
		{
			var allRecipees = new List<IEntityRecipee>();
			var currentRecipee = entityRecipee;
			while (currentRecipee != null)
			{
				allRecipees.Add(currentRecipee);
				currentRecipee = currentRecipee.Parent;
			}
			allRecipees.Reverse();
			return allRecipees;
		}
	}
}