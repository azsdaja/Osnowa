using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.ActionLoop.Actions;
using Moq;
using NUnit.Framework;
using Osnowa;
using Osnowa.Osnowa.Context;
using Osnowa.Osnowa.Core.ECS;

namespace Tests
{
    using IoC;
    using Osnowa.Osnowa.Core;
    using Osnowa.Osnowa.Core.ActionLoop;
    using Osnowa.Osnowa.Unity;
    using UI;
    using Zenject;

    [TestFixture]
    public class FirstTest : ZenjectUnitTestFixture
    {
        private IList<string> _log = new List<string>();
    
        [SetUp]
        public void InitializeContainer()
        {
            var gameConfig = Mock.Of<IGameConfig>();
            var uiManager = Mock.Of<IUiManager>();
            var savedComponents = Mock.Of<ISavedComponents>();
            GameGlobalsInstaller.Install(Container, null, gameConfig, uiManager, savedComponents);

            var unityGridInfoProvider = Mock.Of<IUnityGridInfoProvider>();
            Container.Rebind<IUnityGridInfoProvider>().FromInstance(unityGridInfoProvider);

            IActionEffectFactory actionEffectFactory = Container.Resolve<IActionEffectFactory>();
            var actionResolverMock = new Mock<IActionResolver>();
            actionResolverMock.Setup(r => r.GetAction(It.IsAny<GameEntity>())).Returns<GameEntity>(
                entity => new LambdaAction(entity, 1f, actionEffectFactory, (e) =>
                {
                    _log.Add($"Tu {e.id.Id}, mam akcję! Energii {e.energy.Energy}");
                    return Enumerable.Empty<IActionEffect>();
                }));
            Container.UnbindId<IActionResolver>("_actionResolver");
            Container.Bind<IActionResolver>().WithId("_actionResolver").FromInstance(actionResolverMock.Object);
            Container.ResolveId<IActionResolver>("_actionResolver");
        
            var contextManager = Container.Resolve<IOsnowaContextManager>();
            contextManager.ReplaceContext(new OsnowaContext(1000, 1000));
        
            Contexts.sharedInstance.SubscribeId();
        }
    
        [Test]
        public void RunTest1()
        {
            GameContext context = Contexts.sharedInstance.game;
            var entityGenerator = Container.Resolve<IEntityGenerator>();
            IEntityRecipee recipee = Mock.Of<IEntityRecipee>();
//        entityGenerator.GenerateActorFromRecipeeAndAddToContext(context, recipee, new Position(0, 0), out GameEntity entity);
        
            var entity1 = context.CreateEntity();
            entity1.ReplacePosition(new Position(0, 0));
            entity1.ReplaceEnergy(1f, 0.5f);
        
            var entity2 = context.CreateEntity();
            entity2.ReplaceEnergy(0.3f, 0f);
            entity2.ReplacePosition(new Position(1, 1));

            var turnManager = Container.Resolve<ITurnManager>();
        
            turnManager.OnGameStart();

            for (int i = 1; i < 10; i++)
            {
                _log.Add($"Tura {i}!");   
                turnManager.Update();
            }
        
            Assert.Pass(string.Join(Environment.NewLine, _log));
        }
    }
}