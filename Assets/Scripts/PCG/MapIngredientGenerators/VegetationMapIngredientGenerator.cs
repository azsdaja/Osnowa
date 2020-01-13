namespace PCG.MapIngredientGenerators
{
    using System;
    using System.Collections;
    using System.Linq;
    using MapIngredientConfigs.Vegetation;
    using Osnowa.Osnowa.Core;
    using Osnowa.Osnowa.Example;
    using Osnowa.Osnowa.RNG;
    using UnityEngine;
    using UnityUtilities;

    public class VegetationMapIngredientGenerator : MapIngredientGenerator
    {
        private Plant[,] _plants;
        private int _initialSeeds;

        private ValueMap _soilMap;
        private ValueMap _heightMap;

        private VegetationIngredientConfig _config;

        private IRandomNumberGenerator _rng;
        private MatrixByte[] _tileMatricesByte;

        public void Init(IExampleContext context, VegetationIngredientConfig config,
            WorldGeneratorConfig worldGeneratorConfig, IRandomNumberGenerator rng, ValueMap soilMap, ValueMap heightMap)
        {
            _rng = rng;
            _config = config;
            _soilMap = soilMap;
            _heightMap = heightMap;
            base.Init(context, config, worldGeneratorConfig);

            Values = new ValueMap(1, worldGeneratorConfig.XSize, worldGeneratorConfig.YSize);
            _plants = new Plant[worldGeneratorConfig.XSize, worldGeneratorConfig.YSize];
            _initialSeeds = (int) (worldGeneratorConfig.XSize * worldGeneratorConfig.YSize *
                                   _config.InitialSeedsPerPosition);
            _tileMatricesByte = context.TileMatricesByteByLayer;
        }

        public override IEnumerator Recalculating()
        {
            SpreadInitialSeeds();

            yield return new WaitForSeconds(0.1f);

            int iterations = _config.Iterations;
            for (var iteration = 0; iteration < iterations; iteration++)
            {
                SimulateOneGeneration(iteration < iterations - 2);

                yield return new WaitForSeconds(0.1f);
            }

            for (var x = 1; x < Values.XSize - 1; x++)
            for (var y = 1; y < Values.YSize - 1; y++)
            {
                Plant plant = _plants[x, y];
                if (plant != null)
                {
                    if (plant.Tile == null)
                        throw new ArgumentNullException($"Missing tile for plant {plant.name}.");
                    byte layer = plant.Tile.Layer;
                    byte id = plant.Tile.Id;
                    _tileMatricesByte[layer].Set(x, y, id);

                    if (!plant.GrowsBelowOtherPlants) //not grass layer
                    {
                        Plant neighbourGrowingBelow = PositionUtilities.Neighbours8(new Position(x, y))
                            .Select(n => _plants[n.x, n.y])
                            .FirstOrDefault(p => p != null && p.GrowsBelowOtherPlants);
                        if (neighbourGrowingBelow != null)
                            _tileMatricesByte[neighbourGrowingBelow.Tile.Layer]
                                .Set(x, y, neighbourGrowingBelow.Tile.Id);
                    }
                }
            }
        }

        private void SimulateOneGeneration(bool allowSpreading)
        {
            for (var x = 0; x < Values.XSize; x++)
            for (var y = 0; y < Values.YSize; y++)
            {
                Plant plant = _plants[x, y];
                if (plant != null)
                {
                    float scoreFromSoil = ScoreFromSoil(plant, _soilMap.Get(new Position(x, y)));
                    float scoreFromNeighbours = GetScoreFromNeighbours(plant, x, y);
                    float randomFactorScore = _rng.NextFloat() - 0.5f;

                    float totalScore = scoreFromSoil + scoreFromNeighbours + randomFactorScore;

                    if (totalScore < plant.ScoreToDie)
                    {
                        KillPlant(x, y, plant);
                    }
                    else
                    {
                        bool shouldSpread = allowSpreading && totalScore > plant.ScoreToSpreadSeeds;
                        if (shouldSpread)
                            for (var i = 0; i < plant.SeedsAmount; i++)
                            {
                                int newSeedX = x + _rng.Next(1, plant.SeedsSpread) * _rng.Sign();
                                int newSeedY = y + _rng.Next(1, plant.SeedsSpread) * _rng.Sign();
                                if (newSeedX < 0 || newSeedY < 0 || newSeedX >= Values.XSize ||
                                    newSeedY >= Values.YSize)
                                    continue;
                                if (_plants[newSeedX, newSeedY] == null) SetPlant(newSeedX, newSeedY, plant);
                            }
                    }
                }
            }
        }

        private float GetScoreFromNeighbours(Plant plant, int plantX, int plantY)
        {
            int plantRootsRange = plant.RootsRange;
            int totalNutritionAvailable = (plantRootsRange + 1) * (plantRootsRange + 1);
            float nutritionTakenByNeighbours = 0;
            float scoreFromSympathyToNeighbours = 0;
            for (int x = plantX - plantRootsRange; x <= plantX + plantRootsRange; x++)
            for (int y = plantY - plantRootsRange; y <= plantY + plantRootsRange; y++)
            {
                if (x < 0 || y < 0 || x >= Values.XSize || y >= Values.YSize
                    || x == plantX && y == plantY)
                    continue;
                Plant neighbourPlant = _plants[x, y];
                if (neighbourPlant != null)
                {
                    nutritionTakenByNeighbours += neighbourPlant.NutritionTaken;
                    float likelinessGain = 0.1f / (plantRootsRange + 1);
                    foreach (Plant likedNeighbour in plant.LikedNeighbours.Where(n => n == neighbourPlant))
                        scoreFromSympathyToNeighbours += likelinessGain;

                    foreach (Plant dislikedNeighbour in plant.DislikedNeighbours.Where(n => n == neighbourPlant))
                        scoreFromSympathyToNeighbours -= likelinessGain;
                }
            }

            float score = plant.NeighboursRatioToScore.Evaluate(nutritionTakenByNeighbours / totalNutritionAvailable);

            float height = _heightMap.Get(plantX, plantY);
            score += plant.HeightToScore.Evaluate(height);

            score += scoreFromSympathyToNeighbours;
            return score;
        }

        private void SpreadInitialSeeds()
        {
            for (var i = 0; i < _initialSeeds; i++)
            {
                Position position = _rng.NextPosition(Values.XSize, Values.YSize);
                Plant plant = _rng.Choice(_config.PlantDefinitions);
                SetPlant(position.x, position.y, plant);
            }
        }

        private void SetPlant(int x, int y, Plant plant)
        {
            Plant next = plant.ChildWhenSpreading;
            _plants[x, y] = next;
            Values.Set(new Position(x, y), next.Value);
        }

        private void KillPlant(int x, int y, Plant plant)
        {
            Plant next = plant.ChildWhenDying;
            _plants[x, y] = next;
            if (next == null)
            {
                Values.Set(new Position(x, y), 0);
                return;
            }

            Values.Set(new Position(x, y), next.Value);
        }

        private float ScoreFromSoil(Plant plant, float soilType)
        {
            if (soilType == SoilIngredientGenerator.Soil)
                return plant.ScoreOnSoil;
            if (soilType == SoilIngredientGenerator.Sand)
                return plant.ScoreOnSand;
            if (soilType == SoilIngredientGenerator.None)
                return plant.ScoreOnWater;

            return 0f;
        }
    }
}