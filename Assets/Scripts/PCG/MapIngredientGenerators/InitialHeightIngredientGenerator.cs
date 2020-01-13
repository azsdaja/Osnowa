namespace PCG.MapIngredientGenerators
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using FloodSpill;
	using MapIngredientConfigs;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.RNG;
	using UnityEngine;
	using Position = Osnowa.Osnowa.Core.Position;

	public class InitialHeightIngredientGenerator : MapIngredientGenerator
	{
		private InitialHeightIngredientConfig _config;
		private IRandomNumberGenerator _rng;
		private Position _noiseOffset;

        public void Init(IExampleContext context, InitialHeightIngredientConfig config, WorldGeneratorConfig worldGeneratorConfig, IRandomNumberGenerator rng)
		{
            _config = config;
			_rng = rng;
			_noiseOffset = new Position(_rng.Next(10000), _rng.Next(10000));

			base.Init(context, config, worldGeneratorConfig);

			Values = new ValueMap(1, worldGeneratorConfig.XSize, worldGeneratorConfig.YSize);
		}

		public override IEnumerator Recalculating()
		{
			Position middle = new Position(Values.XSize/2, Values.YSize/2);
			float furthestDistanceToMiddle = Position.Distance(Position.MinValue, middle);
			float noiseScale = _config.Scale;
			AnimationCurve toCenterFalloffCurve = _config.MaxFactorDistanceFromCenterToFalloff;
			Dictionary<int, float> afterNoiseAdjustments = InitializeAfterNoiseAdjustments(_config.Octaves);
			foreach (Position position in Values.AllCellMiddles())
			{
				Position probedPositionForNoise = position + _noiseOffset;

                float persistence01 = 0.5f;

				float heightValue = Perlin.Noise(probedPositionForNoise.x * noiseScale, probedPositionForNoise.y * noiseScale, 
					_config.Octaves, persistence01);
				int adjustmentKey = (int)Math.Round(persistence01 * 10);
				heightValue *= afterNoiseAdjustments[adjustmentKey];

				float distanceToMiddle = Position.Distance(position, middle);
				float relativeDistanceToMiddle = distanceToMiddle/furthestDistanceToMiddle;
				heightValue *= toCenterFalloffCurve.Evaluate(relativeDistanceToMiddle);
				Values.Set(position, heightValue);
			}

			float cutOffValue = CalculateSeaLevelToMatchGroundPercentage();
			GameContext.SeaLevel = cutOffValue;

            bool Qualifier(int x, int y) => Values.Get(x, y) < cutOffValue;

            var bounds = new FloodBounds(Values.XSize, Values.YSize);
			var parameters = new FloodParameters(0, 0)
			{
				Qualifier = Qualifier,
				BoundsRestriction = bounds,
				NeighbourhoodType = NeighbourhoodType.Four
			};
			int[,] closedSeaPositions = new int[bounds.SizeX, bounds.SizeY];
			new FloodSpiller().SpillFlood(parameters, closedSeaPositions);

			BuryDepressions(cutOffValue, closedSeaPositions);

			yield return new WaitForSeconds(0.1f);
		}

		private float CalculateSeaLevelToMatchGroundPercentage()
		{
			int randomPointsCount = 100;
			List<float> allProbedValues = new List<float>(randomPointsCount);
			for (int i = 0; i < randomPointsCount; i++)
			{
				var position = new Position(_rng.Next(Values.XSize), _rng.Next(Values.YSize));
				float value = Values.Get(position);
				allProbedValues.Add(value);
			}

			allProbedValues.Sort();
			allProbedValues.Reverse();

			int cutOffValueIndex = (int) (_config.GroundPercentage * randomPointsCount);
			float cutOffValue = allProbedValues[cutOffValueIndex];
			return cutOffValue;
		}

		private void BuryDepressions(float cutOffValue, int[,] closedSeaPositions)
		{
			foreach (Position position in Values.AllCellMiddles())
			{
				float value = Values.Get(position);
				Func<int, int, bool> isSea = (x, y) => closedSeaPositions[x, y] < int.MaxValue;
				if (value < cutOffValue && !isSea(position.x, position.y))
				{
					float heightReflectedFromCutoff = 2*cutOffValue - value; // to prevent depressions
					Values.Set(position, heightReflectedFromCutoff);
				}
			}
		}

		/// <summary>
		/// A noise with multiple octaves has abominated value (the more octaves, the higher), 
		/// so we have to find out the modifiers that would normalize the values again. In order
		/// not to calculate it for each possible persistence, we do it for 10 values of persistence 
		/// and then will use approximations.
		/// </summary>
		private static Dictionary<int, float> InitializeAfterNoiseAdjustments(int octaves)
		{
			var dictionary = new Dictionary<int, float>();
			for (int i = 0; i <= 10; i += 1)
			{
				float persistence = ((float)i) / 10;
				float maxValueFromNoise = Perlin.MaxValueFromNoise(octaves, persistence: persistence);
				float maxValueFromNoiseInverted = 1 / maxValueFromNoise;
				dictionary[i] = maxValueFromNoiseInverted;
			}

			return dictionary;
		}
	}
}