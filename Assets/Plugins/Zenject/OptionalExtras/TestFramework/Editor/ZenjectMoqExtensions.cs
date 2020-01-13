#if UNITY_EDITOR && !UNITY_WEBPLAYER
using Moq;
#endif

namespace Zenject
{
    public static class ZenjectMoqExtensions
    {
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder FromMock<TContract>(this FromBinderGeneric<TContract> binder)
            where TContract : class
        {
#if UNITY_EDITOR && !UNITY_WEBPLAYER
            return binder.FromInstance(Mock.Of<TContract>());
#else
            Assert.That(false, "The use of 'ToMock' in web builds is not supported");
            return null;
#endif
        }

        public static ConditionCopyNonLazyBinder FromMock<TContract>(this FactoryFromBinder<TContract> binder)
            where TContract : class
        {
            return binder.FromInstance(Mock.Of<TContract>());
        }
    }
}
