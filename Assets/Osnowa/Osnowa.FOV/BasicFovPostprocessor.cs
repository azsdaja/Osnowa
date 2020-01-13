namespace Osnowa.Osnowa.FOV
{
	using System;
	using System.Collections.Generic;
	using Core;
	using UnityEngine;
	using UnityUtilities;

	public class BasicFovPostprocessor : IBasicFovPostprocessor
	{
		public IEnumerable<Position> PostprocessBasicFov(
			HashSet<Position> visibleBeforePostProcessing, Position centerOfSquareToPostProcess, int rayLength, Func<Position, bool> isWalkable)
		{
			int rayLengthSquared = rayLength * rayLength;
		
			var boundsMinCorner = new Vector3Int(centerOfSquareToPostProcess.x - rayLength, centerOfSquareToPostProcess.y - rayLength, 0);
			int fovSquareSize = 2 * rayLength + 1; // two rayLengths plus one unit for center
			BoundsInt boundsToPostProcess = new BoundsInt(boundsMinCorner, new Vector3Int(fovSquareSize, fovSquareSize, 1));
			foreach (Vector3Int position3D in boundsToPostProcess.allPositionsWithin)
			{
				var currentPosition = new Position(position3D.x, position3D.y);

				bool isWithinRay = (currentPosition - centerOfSquareToPostProcess).SqrMagnitude <= rayLengthSquared;
				bool canBeIlluminated = 
					!visibleBeforePostProcessing.Contains(currentPosition) 
					&& isWithinRay 
					&& !isWalkable(currentPosition);
				if (!canBeIlluminated)
					continue;

				IEnumerable<Position> behindnessVectors = GetBehindnessVectors(currentPosition, centerOfSquareToPostProcess);
				foreach (var behindnessVector in behindnessVectors)
				{
					Position potentialIlluminatingPosition = currentPosition - behindnessVector;
					bool potentialIlluminatingNeighbourIsLitAndNonBlocking 
						= visibleBeforePostProcessing.Contains(potentialIlluminatingPosition) && isWalkable(potentialIlluminatingPosition);
					if (potentialIlluminatingNeighbourIsLitAndNonBlocking)
					{
						yield return currentPosition;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Returns normalized vectors that point from a tile to the tile that can be considered to be "behind" it.
		/// </summary>
		internal IEnumerable<Position> GetBehindnessVectors(Position currentPosition, Position centerOfSquareToPostProcess)
		{
			Position direction = currentPosition - centerOfSquareToPostProcess;
			var snappedToX = PositionUtilities.SnapToXAxisNormalized(direction);
			var snappedToY = PositionUtilities.SnapToYAxisNormalized(direction);
			if(snappedToX != Position.Zero) yield return snappedToX;
			if(snappedToY != Position.Zero) yield return snappedToY;
		}
	}
}