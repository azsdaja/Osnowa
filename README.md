# Osnowa
a roguelike framework for C# with Unity integration

# Introduction
Osnowa's goal is to simplify creation of roguelike games. It's divided into modules that are supposed to be as much decoupled as possible, but the easiest way to create a new game is to use the full framework with its Unity integration and modify the code of the template game which is attached to the project.
The framework code is only little coupled to Unity. However the Unity implementation is the main focus, so current version of documentation will describe the implementation with an assumption of using Unity.

# Features
The easiest way to see what can be made with this is to check out the games I made for 7DRL 2019 and 2018: Quinta essentia and Artifex gladii.
This is a rough list of features:
- ECS architecture (using [Entitas](https://github.com/sschmid/Entitas-CSharp),
- pathfinding using JPS and A*,
- Field of View (FOV) calculation,
- utility-based AI basing on pluggable skills which represent coded activities,
- energy-based turn management system (following [Robert Nyström's article](http://journal.stuffwithstuff.com/2014/07/15/a-turn-based-game-loop/)),
- a single grid-based map integrated with Unity's 2D tilemaps,
- fast and memory-efficient flood runs / Dijsktra maps (using [FloodSpill](https://github.com/azsdaja/FloodSpill-CSharp) library),
- map generator (loosely based on [Amit Patel's article](http://www-cs-students.stanford.edu/~amitp/game-programming/polygon-map-generation/)),
- storing game assets like configuration and entity definitions using Unity's [ScriptableObjects](http://minhhh.github.io/posts/understanding-unity-scriptableobject).
- generator of Unity neighbourhood-aware tiles based on prepared tilesheet

Some assets I'm using in the project are:
- open-source tilesets: [DawnLike](https://opengameart.org/comment/60159)
- [Extenject](https://github.com/svermeulen/Extenject) (a Zenject fork) for dependency injection

# Getting started

Currently the easiest way for getting familiar with Osnowa is to open the example project built on top of it, look around and get familiar with it. [This instruction](https://github.com/azsdaja/Osnowa/blob/PreparingDocs/Docs/Looking%20around/Looking%20around.md) will guide you around the project.

# How does Osnowa work?
- ...
- it's monster-agnostic; at any moment you can make the player control a different entity; however, at the moment there is no mechanism to resolve the set of actions that given entity can take; each entity, if is controlled by player, can attempt to perform any action that the player could.

# Core concepts
- 

# Packages
- Osnowa.Core - contains the most basic elements of the framework used widely in other packages,
- Osnowa.Context - //to chyba razem z Core?
- Osnowa.AI - //szczątkowe, tylko czynności
- Osnowa.Entities - //szczątkowe
- Osnowa.Fov - a module designated for Field of View calculation,
- Osnowa.Grid - //
- Osnowa.Pathfinding - pathfinding-related stuff
- Osnowa.Rng - provides random generation tool for numbers, positions, time, collection elements etc.
- Osnowa.Tiles - //szczątkowe, Tileset do Example a TilePresenter zarządza kafelkami pod kątem widoczności. Może do FOV?
- Osnowa.Unity - contains implementations of some Osnowa interfaces working with Unity API

- Osnowa.Example - contains classes used by an example game built upon Osnowa

# The tilemap
The tilemap is stored in two-dimensional arrays, one per each layer. Each layer corresponds to one Unity Tilemap object in the scene. Each layer is dedicated for different types of tiles, for example water (seas, lakes, rivers), soil (swamp, sand or well, soil), floor tiles (grass, roads, floors), standing tiles (trees, bushes, barrels, furniture), decoration tiles (fog, flowers etc.). A layer can have exactly one tile at given position. Tile data of a layer is stored in a two-dimensional byte matrix. There is <> class that can update the Unity tilemap according to changes in Osnowa tiles.

# ECS
ECS in Osnowa is handled by _Entitas_ library. It's quite powerful, performant and has good documentation. In short, to create and use a new component with Entitas, you have to:
1. Create a class which implements IComponent interface and defines public fields that represent the component data,
2. In Unity run Entitas code generator from Tools —> Jenny —> Generate. The code must compile for it to work.
3. Now Entitas will regenerate code for using the components. This code if placed in classes named like GameXyzComponent. They are all defining a partial GameEntity class and methods or properties that let you manipulate the component values. See the example:
[przykład z komponentem z dwoma polami, np. energią oraz z logicznym]

# Core components
# GameActions
A GameAction is an object representing a single 

# ScriptableObjects

# AI
# Activities
Activity is an object representing a task that an actor is occupied with at the moment. Each time a turn is given to an actor, it uses its current activity to generate a GameAction that the actor will perform.
# Skills
Osnowa uses utility-based AI. Each actor (an entity having AI component[?]) has a list of Skills defining what he can do. Each skill has a SkillEvaluator, a list of conditions and ActivityCreator. SkillEvaluator calculates the utility score (0.0-1.0 number) of given skill basing on context. When an actor needs to decide what action it should take, it goes through all of the skills it has, chooses the ones which conditions are satisfied, runs their evaluators and chooses the skill that had the highest score. Then, the ActivityCreator is used to create an activity that will be assigned to the actor.

# Game initialization

# Turn management
Turn management in Osnowa is based on the concept presented in _Robert Nyström's article_, similar to energy system in ADOM. In short, entities may have Energy component which indicates their current energy and energy gain per turn (or “segment”). An entity with more than 1.0 energy perform an action that consumes some of it (usually exactly 1.0 which means one typical turn). Then they have to wait unless they regain their energy again.
The class TurnManager handles the game loop. Each time its Update() function is called (it's basically like Update() function of any Unity GameObject), it checks the entities with Energy component, chooses the ones that have more than 1.0 energy and gives them initiative, one by one. An entity with initiative will use it's EntityController to execute an action. If no action is executed (for example because the entity is controlled by player and it's waiting for input or because an animation of its previous action is still running), the initiative will NOT be passed to next entity. The process will be repeated in next Update calls.

# Map generator


# UI
Current UI is a stub. It contains just a few elements. It should be accesed by IUiController or entity's IViewController.
# Message log
Message log is used by 

# Entity generation
An entity is a set of components, so to generate an entity you have create one using the GameContext class of Entitas and add some components to it. An easy way for spawning entities matching a given pattern is using EntityGenerator class and passing an EntityRecipee to it. An EntityRecipee is a ScriptableObject containing 
- a list of sprites, among which w random one will be chosen for the entity,
- a list of ComponentRecipees which will create components for it,
- a list of skills that the generated entity will have, 
- a parent EntityRecipee that the current recippe will derive the ComponentRecipees and Skills of.
For a newly created entity that has both Position and Looks components, a []System will automatically generate a Unity GameObject and initialize it.

The game data is stored in entities (handled by Entitas), 
