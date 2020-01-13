namespace Tests.Pathfinding
{
	using System.Text;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Grid;
	using UnityEngine;
	using Bounds = Osnowa.Osnowa.Core.Bounds;

	public class TestGrid : IGrid
	{
		public enum GridDensity { Sparse, Medium, Dense }

		private readonly GridDensity _density;
		private readonly bool _hasWalls;
//		private readonly Noise2D _noise;
		private readonly bool[,] _walkability;
		public int XSize { get; }
		public int YSize { get; }
		public Position MinPosition { get; set; }
		public float CellSize { get; set; }
		public Bounds Bounds { get; set; }

		public TestGrid(int xSize, int ySize, GridDensity density, bool hasWalls, int? seed = null)
		{
			_density = density;
			_hasWalls = hasWalls;
//			_noise = seed == null ? new Noise2D() : new Noise2D(seed.Value);
			XSize = xSize;
			YSize = ySize;
			MinPosition = new Position(0, 0);
			Bounds = new Bounds(MinPosition.x, MinPosition.y, xSize, ySize);
			_walkability = GenerateWalkabilityMatrix(xSize, ySize);
		}

		private bool[,] GenerateWalkabilityMatrix(int xSize, int ySize)
		{
			var walkability = new bool[xSize, ySize];

			float perlinScale = .5f;
			for (int x = 0; x < xSize; x++)
			{
				for (int y = 0; y < ySize; y++)
				{
					float xScaled = x * perlinScale;
					float yScaled = y * perlinScale;
					float threshold = _density == GridDensity.Sparse ? 0.6f : _density == GridDensity.Medium ? 0.55f : 0.5f;

					float value = .5f;//_noise.NoiseComplex(xScaled, yScaled);
					if (value < threshold)
						walkability[x, y] = true;
				}
			}

			if (_hasWalls)
			{
				for (int x = 0; x < xSize; x++)
				{
					for (int y = 0; y < ySize; y++)
					{
						float xScaled = x * perlinScale * 0.08f;
						float yScaled = y * perlinScale;

						float value = 0.5f;//_noise.Noise(xScaled, yScaled);
						float value2 = 0.5f;//_noise.Noise(yScaled + 3, xScaled + 42);
						if (value < 0.3f)
							walkability[x, y] = false;
						if (value2 < 0.3f)
							walkability[y, x] = false;
					}
				}
			}

			return walkability;
		}

		public string Walkability()
		{
			var result = new StringBuilder();

			for (int y = 0; y < _walkability.GetLength(1); y++)
			{
				for (int x = 0; x < _walkability.GetLength(0); x++)
				{
					result.Append(IsWalkable(new Position(x, y)) ? "." : "#");
				}
				result.Append("\r\n");
			}
			return result.ToString();
		}


		public bool IsWalkable(Position position)
		{
			return _walkability[position.x, position.y];
		}

		/// <inheritdoc />
		public bool IsWalkableChecked(Position position)
		{
			throw new System.NotImplementedException();
		}

		public bool IsWalkable(int x, int y)
		{
			return IsWalkable(new Position(x, y));
		}

		public bool IsPassingLight(Position position)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public bool IsPassingLightChecked(Position position)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public float GetWalkability(Position position)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public void InitializePathfindingData(bool initializeJps)
		{
			throw new System.NotImplementedException();
		}

		public Vector3Int LocalToCell(Vector3 localPosition)
		{
			throw new System.NotImplementedException();
		}

		public Vector3Int WorldToCell(Vector3 worldPosition)
		{
			throw new System.NotImplementedException();
		}

		public Vector3 GetCellCenterWorld(Position cellPosition)
		{
			throw new System.NotImplementedException();
		}
	}
}