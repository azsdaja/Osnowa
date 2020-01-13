namespace GameLogic.AI
{
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ECS;
	using Osnowa.Osnowa.RNG;
	using Osnowa.Osnowa.Unity;
	using UnityEngine;

	public class SoundPlayer
	{
		
		private readonly IGameConfig _gameConfig;
		private readonly IRandomNumberGenerator _rng;
		private readonly GameContext _context;
		private readonly IUnityGridInfoProvider _unityGridInfoProvider;

		public SoundPlayer(IGameConfig gameConfig, IRandomNumberGenerator rng, GameContext context,
			IUnityGridInfoProvider unityGridInfoProvider)
		{
			
			_gameConfig = gameConfig;
			_rng = rng;
			_context = context;
			_unityGridInfoProvider = unityGridInfoProvider;
		}

		private void Play(AudioClip soundToPlay)
		{
			Position playerPosition = _context.GetPlayerEntity().position.Position;
			Vector3 playerWorldPosition = _unityGridInfoProvider.GetCellCenterWorld(playerPosition);
			AudioSource.PlayClipAtPoint(soundToPlay, playerWorldPosition);
		}
	}
}