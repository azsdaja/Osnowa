using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Piec.Tests.GridRelated.TilemapAffecting
{
	[TestFixture]
	public class TilePresenterTests
	{
		// naming explanation for the tests down below:
		// lit tile - a tile that has been specially marked (e.g. by color) due to its visibility
		// visible tile - tile that Field of View algorithm returned

		class SceneContextMock : ISceneContext
		{
			public Light SkyLight { get; }
			public Transform EffectTilemapsParent { get; }
			public Transform ActorsParent { get; }
			public Transform ItemsParent { get; }
			public Transform MapsParent { get; }
			public Grid GameGrid { get; }
			public Tilemap TilemapDefiningOuterBounds { get; }
			public Tilemap FogOfWarTilemap { get; }
			public Tilemap UnseenMaskTilemap { get; }
			public Tilemap StandingTilemap { get; }
			public Tilemap DirtTilemap { get; }
			public Tilemap WaterTilemap { get; }
			public Tilemap DecorationsTilemap { get; }
			public Tilemap FloorsTilemap { get; }
			public IList<Tilemap> AllPresentableTilemaps { get; }
			public IList<Tilemap> AllTilemapsByLayers { get; }
			public HashSet<Position> VisiblePositions { get; set; }
			public Tilemap SoilTilemap { get; }
			public Tilemap RoofTilemap { get; }
			public Tilemap OnRoofTilemap { get; }
			public Tilemap Debug1Tilemap { get; }
			public Tilemap Debug2Tilemap { get; }
			public Tilemap Debug3Tilemap { get; }
		}

		[Test]
		public void Illuminate_NoTilesWereLitAndNoTilesBecomeVisible_NoTilesAreLitAfterwardsAndNoTilesGetAffected()
		{
			var gameContext = new SceneContextMock {VisiblePositions = new HashSet<Position>()};
			var presenterMock = CreateTilePresenterMockWithRealIlluminateImplementation(gameContext);

			presenterMock.Object.UpdateIllumination(new HashSet<Position>());
			var presenter = presenterMock.Object;

			presenter.LitPositionsSaved.Should().BeEmpty();
			presenterMock.Verify(a => a.SetUnseenMask(It.IsAny<Position>()), Times.Never);
		}

		[Test]
		public void Illuminate_SomeTilesWereLitButNoTilesAreVisible_NoTilesAreLitAfterwards()
		{
			var gameContext = new SceneContextMock {VisiblePositions = new HashSet<Position>()};
			var presenterMock = CreateTilePresenterMockWithRealIlluminateImplementation(gameContext);
			var presenter = presenterMock.Object;

			presenter.UpdateIllumination(new HashSet<Position>());

			presenter.LitPositionsSaved.Should().BeEmpty();
		}

		[Test]
		public void Illuminate_SomeTilesWereLitButNoTilesAreVisible_PreviouslyLitTilesBecomeUnlit()
		{
			var firstLitTile = new Position(3,3);
			var secondLitTile = new Position(4,4);
			var gameContext = new SceneContextMock {VisiblePositions = new HashSet<Position>(new[]{firstLitTile, secondLitTile})};
			var presenterMock = CreateTilePresenterMockWithRealIlluminateImplementation(gameContext);
			var presenter = presenterMock.Object;

			presenter.UpdateIllumination(new HashSet<Position>());

			presenterMock.Verify(a => a.SetUnseenMask(It.IsAny<Position>()), Times.Exactly(2));
			presenterMock.Verify(a => a.SetUnseenMask(firstLitTile), Times.Once);
			presenterMock.Verify(a => a.SetUnseenMask(secondLitTile), Times.Once);
		}

		[Test]
		public void Illuminate_NoTilesWereLitAndSomeTilesBecomeVisible_VisibleTilesAreAffectedAndLitAfterwards()
		{
			var firstVisibleTile = new Position(1, 1);
			var secondVisibleTile = new Position(2, 2);
			var gameContext = new SceneContextMock {VisiblePositions = new HashSet<Position>()};
			var presenterMock = CreateTilePresenterMockWithRealIlluminateImplementation(gameContext);
			var litTiles = new HashSet<Position>{ firstVisibleTile, secondVisibleTile };
			var presenter = presenterMock.Object;

			presenter.UpdateIllumination(litTiles);

			presenter.LitPositionsSaved.Should().BeEquivalentTo(litTiles);
			presenterMock.Verify(a => a.SetUnseenMask(It.IsAny<Position>()), Times.Exactly(2));
			presenterMock.Verify(a => a.SetUnseenMask(firstVisibleTile), Times.Once);
			presenterMock.Verify(a => a.SetUnseenMask(secondVisibleTile), Times.Once);
		}

		[Test]
		public void Illuminate_TwoTilesAreVisibleAndOneOfThemWasAlreadyLit_NewlyLitTileBecomesLitAndIsAffected()
		{
			var firstVisibleTile = new Position(1, 1);
			var secondVisibleTile = new Position(2, 2);
			var initiallyLitTiles = new HashSet<Position>{firstVisibleTile};
			var gameContext = new SceneContextMock { VisiblePositions = new HashSet<Position>() };
			var presenterMock = CreateTilePresenterMockWithRealIlluminateImplementation(gameContext);
			gameContext.VisiblePositions = initiallyLitTiles;
			var visibleTiles = new HashSet<Position>{ firstVisibleTile, secondVisibleTile };
			var presenter = presenterMock.Object;

			presenter.UpdateIllumination(visibleTiles);

			presenter.LitPositionsSaved.Should().BeEquivalentTo(visibleTiles);
			presenterMock.Verify(a => a.SetUnseenMask(It.IsAny<Position>()), Times.Once);
			presenterMock.Verify(a => a.SetUnseenMask(secondVisibleTile), Times.Once);
		}

		[Test]
		public void Illuminate_TwoTilesWereLitButOnlyOneIsVisible_OtherTileBecomesUnlitAndIsAffected()
		{
			var firstVisibleTile = new Position(1, 1);
			var secondVisibleTile = new Position(2, 2);
			var gameContext = new SceneContextMock { VisiblePositions = new HashSet<Position>() };
			var presenterMock = CreateTilePresenterMockWithRealIlluminateImplementation(gameContext);
			var initiallyLitTiles = new HashSet<Position>{firstVisibleTile, secondVisibleTile };
			var visibleTiles = new HashSet<Position>{ secondVisibleTile };
			gameContext.VisiblePositions = initiallyLitTiles;
			var presenter = presenterMock.Object;

			presenter.UpdateIllumination(visibleTiles);

			presenter.LitPositionsSaved.Should().BeEquivalentTo(visibleTiles);
			presenterMock.Verify(a => a.SetUnseenMask(It.IsAny<Position>()), Times.Once);
			presenterMock.Verify(a => a.SetUnseenMask(firstVisibleTile), Times.Once);
		}

		private static Mock<TilePresenter> CreateTilePresenterMockWithRealIlluminateImplementation(SceneContextMock sceneContext)
		{
			var mock = new Mock<TilePresenter>(sceneContext) {CallBase = true};
			mock.Setup(m => m.SetUnseenMask(It.IsAny<Position>()));
			return mock;
		}
	}
}