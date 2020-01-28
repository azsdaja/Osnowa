# Dependency injection

Osnowa uses [Extenject](https://github.com/svermeulen/Extenject) (a Zenject fork) for dependency injection. Check out its docs to get familiar with the concept. If you're new to DI, they have a [nice theoretical introduction to this subject](https://github.com/svermeulen/Extenject#theory).

Constructor injection is widely used in the project as a way of passing dependencies. An example is `EntityGenerator`:
``` csharp
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
    ...
    }
```

This grants us decoupling which makes it easier to test code and replace behaviour.

`GameGlobalsInstaller` is the class that defines configuration of bindings. Most classes are automatically bound to interfaces they implement. If you need to bind something to an instance, to use binding identifiers and so on, you have to exclude your type from automatic binding (in `CreateAutomaticBindings()`) and use manual binding instead: `Container.Bind<IMyInterface>().FromInstance(myInstance).AsSingle();`.

# DI and actions

`GameAction`s mix data and behaviour and are created not at DI root, but from game logic (from classes implementing `IActionResolver`). That's why to keep the flexibility you need to use a factory. `ActionFactory` lets you create actions of given types with given parameters while still being able to pass them dependencies from DI container.