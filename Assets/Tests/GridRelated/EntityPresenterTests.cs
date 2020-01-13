namespace Tests.GridRelated
{
/*
	[TestFixture]
	public class EntityPresenterTests
	{
		[Test]
		public void GetVisibleEntities_NoPotentiallyVisibleEntities_ReturnsEmptyCollection()
		{
			var presenter = new EntityPresenter(Mock.Of<ISceneContext>());

			IEnumerable<IOldGameEntity> result = presenter.GetVisibleEntities(It.IsAny<HashSet<Position>>(), Enumerable.Empty<IOldGameEntity>());

			result.Should().BeEmpty();
		}

		[Test]
		public void GetVisibleEntities_ThereArePotentiallyVisibleEntitiesButNoVisibleTiles_ReturnsEmptyCollection()
		{
			var presenter = new EntityPresenter(Mock.Of<ISceneContext>());
			IOldGameEntity entity = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(3,5));

			IEnumerable<IOldGameEntity> result = presenter.GetVisibleEntities(new HashSet<Position>(), new []{entity});

			result.Should().BeEmpty();
		}

		[Test]
		public void GetVisibleEntities_ThereArePotentiallyVisibleEntitiesAtSomeMatchingTiles_ReturnsEntitiesOnVisibleTiles()
		{
			var presenter = new EntityPresenter(Mock.Of<ISceneContext>());
			IOldGameEntity entity1 = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(1,1));
			IOldGameEntity entity2 = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(2,2));
			IOldGameEntity entity3 = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(3,3));
			HashSet<Position> visibleTiles = new HashSet<Position>{new Position(1,1), new Position(2,2)};

			IEnumerable<IOldGameEntity> result = presenter.GetVisibleEntities(visibleTiles, new []{ entity1 , entity2, entity3});

			result.Should().BeEquivalentTo(entity1, entity2);
		}

		[Test]
		public void Illuminate_ThereArePotentiallyVisibleEntitiesAndSomeAreOnVisibleTiles_EntitiesOnVisibleTilesGetShown()
		{
			IOldGameEntity entity1 = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(1, 1));
			Mock<IOldGameEntity> entity1Mock = Mock.Get(entity1);
			IOldGameEntity entity2 = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(2, 2));
			Mock<IOldGameEntity> entity2Mock = Mock.Get(entity2);
			IOldGameEntity entity3 = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(3, 3));
			Mock<IOldGameEntity> entity3Mock = Mock.Get(entity3);
			HashSet<Position> visibleTiles = new HashSet<Position> { new Position(1, 1), new Position(2, 2) };
			var visibleEntitiesInGameContext = new HashSet<IOldGameEntity>(new List<IOldGameEntity>());
			var gameContext = Mock.Of<ISceneContext>(c => c.VisibleEntities == visibleEntitiesInGameContext);
			var presenter = new EntityPresenter(gameContext);

			presenter.Illuminate(visibleTiles, new[] { entity1Mock.Object, entity2Mock.Object, entity3Mock.Object });

			entity1Mock.Verify(e => e.Show(), Times.Once);
			entity2Mock.Verify(e => e.Show(), Times.Once);
			entity3Mock.Verify(e => e.Show(), Times.Never);
		}

		[Test]
		public void Illuminate_SomeEntitiesGetShownWithSameIllumination_TheyGetShownOnlyOnceAndDontGetHidden()
		{
			IOldGameEntity entity1 = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(1, 1));
			Mock<IOldGameEntity> entity1Mock = Mock.Get(entity1);
			IOldGameEntity entity2 = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(2, 2));
			Mock<IOldGameEntity> entity2Mock = Mock.Get(entity2);
			IOldGameEntity entity3 = Mock.Of<IOldGameEntity>(e => e.EntityData.LogicalPosition == new Position(3, 3));
			Mock<IOldGameEntity> entity3Mock = Mock.Get(entity3);
			HashSet<Position> visibleTiles = new HashSet<Position> { new Position(1, 1), new Position(2, 2) };
			var visibleEntitiesInGameContext = new HashSet<IOldGameEntity>(new List<IOldGameEntity>());
			var gameContext = Mock.Of<ISceneContext>(c => c.VisibleEntities == visibleEntitiesInGameContext);
			var presenter = new EntityPresenter(gameContext);

			presenter.Illuminate(visibleTiles, new[] { entity1Mock.Object, entity2Mock.Object, entity3Mock.Object });
			presenter.Illuminate(visibleTiles, new[] { entity1Mock.Object, entity2Mock.Object, entity3Mock.Object });

			entity1Mock.Verify(e => e.Show(), Times.Once);
			entity1Mock.Verify(e => e.Hide(), Times.Never);
			
			entity2Mock.Verify(e => e.Show(), Times.Once);
			entity2Mock.Verify(e => e.Hide(), Times.Never);

			entity3Mock.Verify(e => e.Show(), Times.Never);
			entity3Mock.Verify(e => e.Hide(), Times.Never);
		}
	}
*/
}