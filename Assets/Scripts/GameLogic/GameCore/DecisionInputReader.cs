namespace GameLogic.GameCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AI.Model;
	using Osnowa;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Core.ECS;
	using Osnowa.Osnowa.Example.ECS.Abilities;
	using Osnowa.Osnowa.Example.ECS.Inventory;
	using UnityEngine;
	using UnityUtilities;
	using Zenject;

	public class DecisionInputReader : MonoBehaviour
	{
		private IInputWithRepeating _inputWithRepeating;
		private IEnvironmentPresenter _environmentPresenter;
		private GameContext _context;
		private IUiFacade _uiFacade;
		private ISceneContext _sceneContext;
		private IOsnowaContextManager _contextManager;

		private IList<KeyValuePair<KeyCode, Ability>> _keysToAbilities;
		[SerializeField] private DirectionInputReader _directionInputReader;
		private IGameConfig _gameConfig;
		private IActionResolver _playerActionResolver;
		private ResolveAbilitiesPerTurnSystem _resolveAbilitiesPerTurnSystem;
		private PlayerInventoryChangedSystem _playerInventoryChangedSystem;

		[Inject]
		public void Init(IInputWithRepeating inputWithRepeating, IEnvironmentPresenter environmentPresenter,
			GameContext context, IGameConfig gameConfig, IUiFacade uiFacade, ISceneContext sceneContext,
			[Inject(Id = "_playerActionResolver")] IActionResolver playerActionResolver,
			ResolveAbilitiesPerTurnSystem resolveAbilitiesPerTurnSystem,
			PlayerInventoryChangedSystem playerInventoryChangedSystem, IOsnowaContextManager contextManager)
		{
			_keysToAbilities = InitializeKeysToAbilities(gameConfig.Abilities.AllAbilities);
			_inputWithRepeating = inputWithRepeating;
			_environmentPresenter = environmentPresenter;
			_context = context;
			_uiFacade = uiFacade;
			_sceneContext = sceneContext;
			_gameConfig = gameConfig;
			_playerActionResolver = playerActionResolver;
			_resolveAbilitiesPerTurnSystem = resolveAbilitiesPerTurnSystem;
			_playerInventoryChangedSystem = playerInventoryChangedSystem;
			_contextManager = contextManager;
		}

		public void Update()
		{
			if (Input.GetKeyUp(KeyCode.LeftAlt))
			{
				_environmentPresenter.StopShowingCharacterDetails();
			}
			if (Input.GetKeyUp(KeyCode.LeftControl))
			{
				_environmentPresenter.StopShowingStealthDetails();
			}

			bool noKeyIsPressed = !Input.anyKey && !Input.anyKeyDown;//osnowatodo && _contextManager.Current.SimulatedKeyDown == KeyCode.None;
			if (noKeyIsPressed)
			{
				_inputWithRepeating.ResetTime();
				return;
			}

			bool onlyModifierIsBeingHeld = 
				!Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftAlt)
				|| (!Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftControl));
			if (onlyModifierIsBeingHeld)
			{
				_inputWithRepeating.ResetTime();
			}

			Ability chosenAbility = ResolveAbility(_keysToAbilities);
			if (chosenAbility != null)
			{
				_sceneContext.CameraMouseControl.StopFollowingPlayer();
				// _uiFacade.ShowEntityDetails(null); powoduje to chamskie znikanie detali co krok

				IGameAction immediateAction = null;
				GameEntity playerEntity = Contexts.sharedInstance.game.GetPlayerEntity();
				if (false)
				{
					
				}
				// todo clean this up
				/*else if (chosenAbility == _gameConfig.Abilities.Custom1)
				{
					immediateAction = _playerActionResolver.CreateTakeFromInventoryActionIfPossible(playerEntity, 0);
				}
				else
				if (chosenAbility == _gameConfig.Abilities.Custom2)
				{
					immediateAction = _playerActionResolver.CreateTakeFromInventoryActionIfPossible(playerEntity, 1);
				}
				else
				if (chosenAbility == _gameConfig.Abilities.Custom3)
				{
					immediateAction = _playerActionResolver.CreateTakeFromInventoryActionIfPossible(playerEntity, 2);
				}
				else
				if (chosenAbility == _gameConfig.Abilities.Custom4)
				{
					immediateAction = _playerActionResolver.CreateTakeFromInventoryActionIfPossible(playerEntity, 3);
				}
				else
				if (chosenAbility == _gameConfig.Abilities.Custom5)
				{
					immediateAction = _playerActionResolver.CreateTakeFromInventoryActionIfPossible(playerEntity, 4);
				}
				else
				if (chosenAbility == _gameConfig.Abilities.Custom6)
				{
					immediateAction = _playerActionResolver.CreateTakeFromInventoryActionIfPossible(playerEntity, 5);
				}
				else
				if (chosenAbility == _gameConfig.Abilities.Custom7)
				{
					immediateAction = _playerActionResolver.CreateTakeFromInventoryActionIfPossible(playerEntity, 6);
				}
				else
				if (chosenAbility == _gameConfig.Abilities.Custom8)
				{
					immediateAction = _playerActionResolver.CreateTakeFromInventoryActionIfPossible(playerEntity, 7);
				}
				else
				if (chosenAbility == _gameConfig.Abilities.Custom9)
				{
					immediateAction = _playerActionResolver.CreateTakeFromInventoryActionIfPossible(playerEntity, 8);
				}
				else
				if (chosenAbility == _gameConfig.Abilities.PickUp)
				{
					immediateAction = _playerActionResolver.CreateTakeToInventoryActionIfPossible(playerEntity);
				}*/

				if (immediateAction != null)
				{
					foreach (IActionEffect actionEffect in immediateAction.Execute())
					{
						actionEffect.Process();
					}

                    _context.ReplacePlayerDecision(Decision.None, Position.Zero, Position.MinValue);

					/* osnowatodo
                     * if (_contextManager.Current.SimulatedKeyDown != KeyCode.None)
					{
						Ability[] clickableAbilities = new[]
						{
						_gameConfig.Abilities.Custom1, _gameConfig.Abilities.Custom2, _gameConfig.Abilities.Custom3,
						_gameConfig.Abilities.Custom4, _gameConfig.Abilities.Custom5, _gameConfig.Abilities.Custom6,
						_gameConfig.Abilities.Custom7, _gameConfig.Abilities.Custom8, _gameConfig.Abilities.Custom9,
						};

						if (clickableAbilities.Contains(chosenAbility))
						{
							immediateAction = _playerActionResolver.CreateDropActionIfPossible(playerEntity);
							if (immediateAction != null)
							{
								foreach (IActionEffect actionEffect in immediateAction.Execute())
								{
									actionEffect.Process();
								}
								_resolveAbilitiesPerTurnSystem.ExecuteQuinta(playerEntity);
								_playerInventoryChangedSystem.ExecuteQuinta(playerEntity);
							}
						}
					}*/




					return;
				}

				_context.ReplacePlayerDecision(chosenAbility.Decision, Position.Zero, Position.MinValue);

				if (chosenAbility.RequiresDirection)
				{
					if (chosenAbility.name == "Spill")
					{
						if (!_context.GetPlayerEntity().hasEntityHolder || _context.GetPlayerEntity().entityHolder.EntityHeld == Guid.Empty)
						{
							return; // quinta don't show aim when nothing in hands
						}
					}
					_uiFacade.SelectAbility(chosenAbility);

					_directionInputReader.gameObject.SetActive(true);
					gameObject.SetActive(false);
				}
				
				if (chosenAbility.RequiresPosition)
				{
					bool aimIsPossible = true;
					if (!aimIsPossible)
					{
						return; // don't show aim when nothing in hands
					}

					_uiFacade.SelectAbility(chosenAbility);

					_directionInputReader.gameObject.SetActive(true);
					gameObject.SetActive(false);
				}
			}
		}

		private Ability ResolveAbility(IList<KeyValuePair<KeyCode, Ability>> keyMappings)
		{
			foreach (KeyValuePair<KeyCode, Ability> keyToAbility in keyMappings)
			{
				if (keyToAbility.Key == KeyCode.None)
					continue;

				if (Input.GetKeyDown(keyToAbility.Key) // osnowatodo || _contextManager.Current.SimulatedKeyDown == keyToAbility.Key
					||
				    (keyToAbility.Value.AllowRepeatingInput && _inputWithRepeating.KeyDownOrRepeating(keyToAbility.Key)))
				{
					if (keyToAbility.Value.Decision == Decision.Custom0)
					{
						return Input.GetKey(KeyCode.LeftControl) ? keyToAbility.Value : null;
					}
					else return keyToAbility.Value;
				}
			}
			return null;
		}

		private IList<KeyValuePair<KeyCode, Ability>> InitializeKeysToAbilities(List<Ability> allAbilities)
		{
			IEnumerable<KeyValuePair<KeyCode, Ability>> basicKeyMappings = allAbilities
				.Select(ability => new KeyValuePair<KeyCode, Ability>(ability.KeyCode, ability));

			IEnumerable<KeyValuePair<KeyCode, Ability>> alternativeKeyMappings = allAbilities
				.Select(ability => new KeyValuePair<KeyCode, Ability>(ability.AlternativeKeyCode, ability));

			IEnumerable<KeyValuePair<KeyCode, Ability>> alternativeKeyMappings2 = allAbilities
				.Select(ability => new KeyValuePair<KeyCode, Ability>(ability.AlternativeKeyCode2, ability));

			return basicKeyMappings.Union(alternativeKeyMappings).Union(alternativeKeyMappings2).ToList();
		}
	}
}
