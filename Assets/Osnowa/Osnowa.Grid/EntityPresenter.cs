using System.Collections.Generic;
using Osnowa.Osnowa.Context;

namespace Osnowa.Osnowa.Grid
{
	using Core;
	using Entities;
	using GameLogic;

	public class EntityPresenter : IEntityPresenter
	{
		private readonly IOsnowaContextManager _contextManager;
		private readonly IUiFacade _uiFacade;
	    private readonly IPositionedEntityDetector _entityDetector;

	    public EntityPresenter(IOsnowaContextManager contextManager, IUiFacade uiFacade, IPositionedEntityDetector entityDetector)
		{
			_contextManager = contextManager;
			_uiFacade = uiFacade;
		    _entityDetector = entityDetector;
		}

		public void UpdateVisibility(HashSet<Position> visiblePositions, Position observerPosition, int cellsInVisionRange)
		{
			IEnumerable<IPositionedEntity> entitiesInRange = _entityDetector.DetectEntities(observerPosition, cellsInVisionRange);
		    HashSet<IPositionedEntity> visibleEntitiesCalculated = GetVisibleEntities(visiblePositions, entitiesInRange);
		    HashSet<IPositionedEntity> visibleEntitiesInContext = _contextManager.Current.VisibleEntities;

            foreach (IPositionedEntity oldEntity in visibleEntitiesInContext)
			{
				if (visibleEntitiesCalculated.Contains(oldEntity)) continue;
			    oldEntity.Hide();
			}
			foreach (IPositionedEntity currentEntity in visibleEntitiesCalculated)
			{
				if (visibleEntitiesInContext.Contains(currentEntity)) continue;
                currentEntity.Show();

                //osnowatodo - fire a kind of "Noticed" event here?
/*
                if (currentEntity.hasView)
				{
					currentEntity.Show();
					if (!currentEntity.isPlayerControlled && currentEntity.hasStrength)
					{
						_uiFacade.AddLogEntry($"<color=#aaa>You notice a {currentEntity.recipee.RecipeeName}.</color>");
					}
				}
*/
            }
			_contextManager.Current.VisibleEntities = new HashSet<IPositionedEntity>(visibleEntitiesCalculated);
		}

		internal HashSet<IPositionedEntity> GetVisibleEntities(HashSet<Position> visibleTiles, IEnumerable<IPositionedEntity> entitiesInArea)
		{
			var result = new HashSet<IPositionedEntity>();
			foreach (IPositionedEntity entity in entitiesInArea)
			{
				Position entityPositionOnGrid = entity.Position;
				if (visibleTiles.Contains(entityPositionOnGrid))
					result.Add(entity);
			}

			return result;
		}
	}
}