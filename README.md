# Osnowa — a roguelike framework for C# with Unity integration and ECS architecture

# Introduction
The goal of Osnowa is to simplify creation of roguelike games. It provides a set of tools useful in this genre. It's also flexible — the framework code is quite decoupled and you can plug your own implementations of interfaces almost everywhere.

The name (spelled _os**no**vah_) comes from Polish word for [warp](https://en.wikipedia.org/wiki/Warp_and_weft) and indicates Osnowa's purpose to be a solid base for creating grid-based games.

The framework is well integrated with [Unity](https://unity.com) in order not to reinvent the wheel. Unity's features like UI, sprites, tilemaps and assets are used as a presentation and configuration tool. That being said, Osnowa's code is not very tightly bound to Unity engine and after some modifications it would still be possible to use it with something else. It's also possible to use only some modules of Osnowa, like pathfinding or field of view calculation.

Osnowa's engine is based on efficient and flexible [Entities-Components-Systems architecture](https://en.wikipedia.org/wiki/Entity_component_system) provided by [Entitas](https://github.com/sschmid/Entitas-CSharp).

If you haven't noticed, Osnowa is free and open-source.

# Made with Osnowa:

The easiest way to see what can be made with Osnowa is to check out the games made for 7DRL 2019 and 2018:

[Quinta essentia](https://pawel-s1.itch.io/quinta-essentia)

<img src="https://img.itch.zone/aW1hZ2UvMzgwNzAzLzE5Mjc1NjkucG5n/original/dXrZq3.png" width=80%/>

[Artifex gladii](https://pawel-s1.itch.io/artifex-gladii)

<img src="https://img.itch.zone/aW1hZ2UvMjMxMjk0LzExMjY1NDQucG5n/original/L7BD8S.png" width=80%/>

# Features
* ECS architecture (using [Entitas](https://github.com/sschmid/Entitas-CSharp),
* pathfinding using JPS and A*,
* Field of View (FOV) calculation,
* utility-based AI basing on pluggable skills which represent coded activities,
* energy-based turn management system (following [Robert Nyström's article](http://journal.stuffwithstuff.com/2014/07/15/a-turn-based-game-loop/)),
* 2D [tilemap](Docs/Tiles.md) with multiple layers integrated with Unity tilemap; auto-generating context-aware RuleTiles,
* fast and memory-efficient flood runs / Dijsktra maps (using [FloodSpill](https://github.com/azsdaja/FloodSpill-CSharp) library),
* map generator (loosely based on [Amit Patel's article](http://www-cs-students.stanford.edu/~amitp/game-programming/polygon-map-generation/)),
* storing game assets like configuration and entity definitions using Unity's [ScriptableObjects](http://minhhh.github.io/posts/understanding-unity-scriptableobject).
* generator of Unity neighbourhood-aware tiles based on prepared tilesheet

Some assets I'm using in the project are:
* open-source tilesets: [DawnLike](https://opengameart.org/comment/60159)
* [Extenject](https://github.com/svermeulen/Extenject) (a Zenject fork) for dependency injection

# State of the project

The framework started its life in 2019 when I realised that after over 1 year of full-time development of my own roguelike game I'm more interested in good architecture and tools than in the game itself. Since then I made decision to publish the good parts of what I've made and make it an open-source project. After a few months of refactoring, polishing and simplifying I reached the current state of the framework where it's ready to be used by the others.

However, **keep in mind it's just the beginning of its public life**. Until now (January 2020) Osnowa hasn't been used by developers other than the author himself. Hopefully it will grow, the documentation will be improved and some problems will be solved. But its future will depend on interest, my free time and contributions of other developers. **Also yours. Engagement in the project will be welcome with open hands.**

The upcoming **[7DRL 2020 challenge](https://itch.io/jam/7drl-challenge-2020)** will be a great occasion to try to make a roguelike with it.

# Getting started

the easiest way to create a new game is to use the full framework with its Unity integration and modify the code of the template game which is attached to the project.


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


