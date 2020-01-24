# Looking around

*somethinhg* —> Unity game object, can be found in the scene
asset —> a file you can find in the project explorer
`something` —> object or class used in code
position —> a cell in two-dimensional grid

This tutorial assumes basic knowledge of Unity engine and editor.

1. Open the project in Unity.
2. Run the game.
3. GameScreensInitializer object enables the NewGameScreen with the initial menu. The latter object contains MapGenerator which is responsible for map generation.
//obrazek
3. Generate a map, play around with different seeds. In WorldGeneratorConfig asset you can modify the map size. Values that make most sense are between 100 and 1000. Frankly, the framework has been only tested with square maps, but you can always try with rectangular ones.
What happens behind the scenes? You can see MapGenerator has dependent objects that are responsible for each step of map generation. The generators are run in given order. Each of them uses `ValueMap` object which basically is a wrapper around a two-dimensional `float` array, usually using value between 0.0 and 1.0. The generators use some data from `ValueMap`s from previous stages to fill their own `ValueMap`s. `ValueMap`s are used only for map generation process. The meaningful data from them is then moved to other classes that Osnowa is using in runtime. Here they maps in creation order:
    a. HeightMap. It uses Perlin noise to create a sensible height map. You can play around with InitialHeightIngredientConfig and change the parameters. ValueToColor gradient let you modify the color depending on height. It calculates the value of `SeaLevel` in `IExampleContext` based on the land ratio defined in the config.
    b. WaterMap. Based on the `SeaLevel`, it puts water tiles on positions that are below it.
    c. SoilMap. It just puts sand or soil tiles on the land based on the height above sea level.
    d. VegetationMap. It creates plant tiles with a simple algorithm simulating multiple generations of spreading plants. It's configured in VegetationIngredientConfig asset and each plant has its config in its own file, for example Tree1.
        - first plants are spread randomly across whole map,
        - then for a few iterations a score is calculated for each plant; if it's less than its ScoreToDie value, the plant dies, optionally leaving another plant in its place; if it's more than ScoreToSpreadSeeds, duplicates of the plant are spread around its vicinity,
        - the score of each plant can be affected be several factors configured in its asset: soil type, height, amount of other plants in vicinity
    Rocks are also generated as "plants".
    e. WalkabilityMap. It just assigns 1 walkability for ground positions and 0 for non-ground positions. It doesn't matter much because later the walkability will be recalculated basing on tiles. But you can modify the code to assign non-binary walkability, which can be useful for further stages. If you have varying walkability, you can for example create twisty roads by running SpatialAStar algorithm by `Pathfinder` class.
    f. BuildingMap. It generates some buildings tiles around the map and places their tiles on it. // to do
    // obrazek
    TileMatricesByLayer property is filled by ... //
4. Start the game. After a while you can see the generated world together with your character. Again, it's good to understand what just happened.
    a. WorldActorFiller class was used to populate the world with the player and other actors. More about entity generation in this section: //.
    b. TilemapGenerator used `TileMatricesByLayer` object in OsnowaContext filled by the map generators to create and present actual Unity tiles. 
    c. Then the MenuMapGenerator deactivated itself revealing the created map.
5. Here's an explanation of some game elements you can see:
    a. The UI is mostly a stub. It's using old good Unity UI objects and components. Have a look at game objects being children of Player UI (for example SideBar) to find out how it's organised and how they interact with each other.
    b. The *GameGrid* object contains Unity *Tilemaps* — one for each layer. Layers are sorted basing on their OrderInLayer properties. Basically, *Water* tilemap is on the bottom and the tilemaps following it are more and more on the top. You can open Tile Palette window in Unity and use Select cursor in order to check what exact tiles are placed on given positions ("Focus on" select button can be helpful). You can even draw tiles if you create a tile palette, but keep in mind that walkability and light-passing properties of positions won't be automatically updated afterwards.
    // zdjęcie
    c. *GameGrid* also contains *Entities*, which is a root of all GameObjects represeting entities in the game. Have a look at *Player* object that should be at the top. It has EntityViewBehaviour and `EntityUiPresenter` components which are used for managing the Unity side of the entity. It also has *Visuals* child object which is used for displaying actor's sprite (*Body*) and UI.
    d. There is also a *Game* object collecting *Entity_number* objects that represent Entitas side of the entities. Click at Entity_0 which is the player entity (you can also just click the player with left mouse button in the Game window). In Unity inspector you can see all Entitas components attached to the player. Here is a description of some of them:
        - Energy is used by Osnowa to give initiative (a turn) to entities. Basically, if an entity has more than 1.0 energy, it will be able to perform an action. Each action costs some energy, typically 1.0. After all entities capable of making actions have performed their turns, their Energy grows by EnergyGainPerSegment value (typically 1.0).
        - PlayerControlled tells Osnowa that given entity is controlled by the player. If you take it from the player, it's entity will act on its own. If you assign it to other entity, you will control it.
        - 
        [ ] przełączanie go nie działa!
    // zdjęcie
    
    

[ ] niech generator używa tych co w preview na generate, a nie wszystkich. A może zrezygnuj z tego? ale niech nie będą wszystkie widoczne zawsze, bo się ujawni za dużo?
[ ] wyczyść konfig morza
