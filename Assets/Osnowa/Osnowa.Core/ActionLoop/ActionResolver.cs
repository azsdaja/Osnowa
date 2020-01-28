using Zenject;

namespace Osnowa.Osnowa.Core.ActionLoop
{
	public class ActionResolver : IActionResolver
	{
		private readonly IActionResolver _aiActionResolver;
		private readonly IActionResolver _playerActionResolver;

		public ActionResolver([Inject(Id = "_aiActionResolver")]IActionResolver aiActionResolver, 
			[Inject(Id = "_playerActionResolver")]IActionResolver playerActionResolver)
		{
			_aiActionResolver = aiActionResolver;
			_playerActionResolver = playerActionResolver;
		}

		public IGameAction GetAction(GameEntity entity)
		{
			if (entity.isPlayerControlled)
			{
				return _playerActionResolver.GetAction(entity);
			}
			return _aiActionResolver.GetAction(entity);
		}
	}
}