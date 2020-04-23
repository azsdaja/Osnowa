# Osnowa — a roguelike framework for C# with ECS and Unity integration
## Introduction
The goal of Osnowa is to simplify creation of roguelike games. It provides a set of tools useful in this genre, as well as a template game. It's also flexible — the framework code is quite decoupled and you can plug your own implementations of interfaces almost everywhere.

The name (spelled _os**noh**vah_) comes from Polish word for [warp](https://en.wikipedia.org/wiki/Warp_and_weft) and indicates Osnowa's purpose to be a solid base for creating grid-based games.

The framework is well integrated with [Unity](https://unity.com) in order not to reinvent the wheel. Unity's features like UI, sprites, tilemaps and assets are used as a presentation and configuration tool. That being said, Osnowa's code is not very tightly bound to Unity engine and after some modifications it would still be possible to use it with something else. It's also possible to use only some modules of Osnowa, like pathfinding or field of view calculation.

Actors in Osnowa engine are based on efficient and flexible [Entities-Components-Systems architecture](https://en.wikipedia.org/wiki/Entity_component_system) provided by [Entitas](https://github.com/sschmid/Entitas-CSharp).

If you haven't noticed yet, Osnowa is free and open-source.

## Made with Osnowa:

The easiest way to see what can be made with Osnowa is to check out these games made for 7DRLs 2018-2020:

[Res furtiva](https://pawel-s1.itch.io/res-furtiva)

<img src="https://img.itch.zone/aW1hZ2UvNTgyMzYxLzMwNzg3MDAucG5n/original/E7zuKW.png" width=60%/>

[Quinta essentia](https://pawel-s1.itch.io/quinta-essentia)

<img src="https://img.itch.zone/aW1hZ2UvMzgwNzAzLzE5Mjc1NjkucG5n/original/dXrZq3.png" width=60%/>

[Artifex gladii](https://pawel-s1.itch.io/artifex-gladii)

<img src="https://img.itch.zone/aW1hZ2UvMjMxMjk0LzExMjY1NDQucG5n/original/L7BD8S.png" width=60%/>

## Features

* ECS architecture (using [Entitas](https://github.com/sschmid/Entitas-CSharp)),
* pathfinding using JPS and A*,
* Field of View (FOV) calculation,
* utility-based AI basing on pluggable skills which represent coded activities,
* energy-based turn management system (following [Robert Nyström's article](http://journal.stuffwithstuff.com/2014/07/15/a-turn-based-game-loop/)),
* 2D [tilemap](https://github.com/azsdaja/Osnowa/wiki/Tiles) with multiple layers integrated with Unity tilemap; auto-generating context-aware RuleTiles,
* fast and memory-efficient flood runs / Dijsktra maps (using [FloodSpill](https://github.com/azsdaja/FloodSpill-CSharp) library),
* map generator (loosely based on [Amit Patel's article](http://www-cs-students.stanford.edu/~amitp/game-programming/polygon-map-generation/)),
* storing game assets like configuration and entity definitions using Unity's [ScriptableObjects](http://minhhh.github.io/posts/understanding-unity-scriptableobject).
* generator of Unity neighbourhood-aware tiles based on prepared tilesheet

Some open-source resources I'm using in the project are:
* tilesets: [DawnLike](https://opengameart.org/comment/60159), [Calciumtrice dungeon](https://opengameart.org/comment/67057)
* [Extenject](https://github.com/svermeulen/Extenject) (a Zenject fork) for dependency injection

## Compatibility
Osnowa is confirmed to be compatible with 2019.2.9-2019.2.21 versions and probably newer ones. It's not compatible with Unity 2018.

## State of the project

The framework started its life in 2019 when I realised that after over 1 year of full-time development of my own roguelike game I'm more interested in good architecture and tools than in the game itself. Since then I made decision to publish the good parts of what I've made and make it an open-source project. After a few months of refactoring, polishing and simplifying I reached the current state of the framework where it's ready to be used by the others.

However, **keep in mind it's just the beginning of its public life**. Until February 2020 Osnowa hasn't been used by developers other than myself (this luckily changed during 7DRL 2020). Hopefully it will grow, the documentation will be improved and some problems will be solved. But its future will depend on interest, my free time and contributions of other developers. **Also yours. Engagement in the project will be welcome with open hands.**

At the current stage of development consider Osnowa to possibly have breaking changes for each minor version (e.g. between 0.1 and 0.2). Of course many changes may still be merged without problems to your project.

## Getting started

Currently the easiest way for getting familiar with Osnowa is to open the example project built on top of it and look around. [**This instruction**](https://github.com/azsdaja/Osnowa/wiki/Looking-around) will guide you around the project. In the [**wiki**](https://github.com/azsdaja/Osnowa/wiki) there are also more docs about specific features.

Then you should be able to modify the code of the template game which is attached to the project or to try taking some parts of Osnowa to your project.

## Contact
Feel free to join the **Gitter chat** if you want to talk about the project: https://gitter.im/Osnowa/community
Here's Twitter account of the creator: https://twitter.com/PawelRoguelikes
