namespace Osnowa.Osnowa.Context
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Core;
	using Core.CSharpUtilities;
	using UnityEngine;
	using UnityUtilities;

	[Serializable]
	public class Area
	{
		[SerializeField] private BoundsInt _bounds;
		[SerializeField] private List<Position> _positions;

		private bool _isDirty;
		[NonSerialized] private List<Position> _perimeter;
		[NonSerialized] private List<Position> _innerPositions;

		public BoundsInt Bounds
		{
			get => _bounds;
            set => _bounds = value;
        }

		public List<Position> Positions
		{
			get => _positions;
            set => _positions = value;
        }

		public List<Position> InnerPositions
		{
			get => _innerPositions == null || _isDirty ? (_innerPositions = RecalculateInnerPositions(_positions, Perimeter)) : _innerPositions;
            set => _innerPositions = value;
        }

		public List<Position> Perimeter
		{
			get => _perimeter == null || _isDirty ? (_perimeter = RecalculatePerimeter(_positions)) : _perimeter;
            set => _perimeter = value;
        }

		public Area(IEnumerable<Position> positions)
		{
			_positions = positions.ToList();
			
			int minX = _positions.Min(p => p.x);
			int minY = _positions.Min(p => p.y);
			int maxX = _positions.Max(p => p.x);
			int maxY = _positions.Max(p => p.y);
            
			Vector3Int size = new Vector3Int(maxX - minX + 1, maxY - minY + 1, 1);
			_bounds = new BoundsInt(new Vector3Int(minX, minY, 0), size);

			_isDirty = true;
		}

		public void AddPosition(Position position)
		{
			_positions.Add(position);
			_bounds = BoundsIntUtilities.With(_bounds, position.ToVector3Int());
			_isDirty = true;
		}

		private List<Position> RecalculatePerimeter(List<Position> positions)
		{
			var positionsSet = positions.ToHashSet();
			if(_perimeter == null)
				_perimeter = new List<Position>();
			else _perimeter.Clear();

			foreach (Position position in positions)
			{
				bool touchesExternalArea = PositionUtilities.Neighbours8(position).Any(n => !positionsSet.Contains(n));
				if(touchesExternalArea)
					_perimeter.Add(position);
			}

			_isDirty = false;
			return _perimeter;
		}

		private List<Position> RecalculateInnerPositions(List<Position> positions, List<Position> perimeter)
		{
			List<Position> positionsWithoutPerimeter = positions.Where(p => !perimeter.Contains(p)).ToList();
			return positionsWithoutPerimeter;
		}
	}
}