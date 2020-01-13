namespace Osnowa.Osnowa.FOV
{
	using System;
	using System.Collections.Generic;
	using Core;

	/// <summary>
	/// A calculator of field of view basing on "Basic" Algorithm described in https://sites.google.com/site/jicenospam/visibilitydetermination.
	/// </summary>
	public class BasicFovCalculator : IFovCalculator
	{
		private readonly IFovSquareOutlineCreator _fovSquareOutlineCreator;
		private readonly IBasicFovPostprocessor _basicFovPostprocessor;
		private readonly IRasterLineCreator _rasterLineCreator;

		public BasicFovCalculator(IFovSquareOutlineCreator fovSquareOutlineCreator, IBasicFovPostprocessor basicFovPostprocessor, 
			IRasterLineCreator rasterLineCreator)
		{
			_basicFovPostprocessor = basicFovPostprocessor;
			_fovSquareOutlineCreator = fovSquareOutlineCreator;
			_rasterLineCreator = rasterLineCreator;
		}

		public HashSet<Position> CalculateFov(Position observerPosition, int sightRange, Func<Position, bool> isPassingLight)
		{
			var positionsInFov = new HashSet<Position>();
			IEnumerable<Position> outlineToCastRaysOn = _fovSquareOutlineCreator.CreateSquareOutline(observerPosition, sightRange);
			foreach (Position point in outlineToCastRaysOn)
			{
				IList<Position> bresenhamLineTiles = _rasterLineCreator
					.GetRasterLine(observerPosition.x, observerPosition.y, point.x, point.y, isPassingLight, sightRange, true);
				positionsInFov.UnionWith(bresenhamLineTiles);
			}

			IEnumerable<Position> visibleTilesFromPostProcessing = 
				_basicFovPostprocessor.PostprocessBasicFov(positionsInFov, observerPosition, sightRange, isPassingLight);

            positionsInFov.UnionWith(visibleTilesFromPostProcessing);

		    return positionsInFov;

		}
	}
}