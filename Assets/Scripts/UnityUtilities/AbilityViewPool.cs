namespace UnityUtilities
{
	using UI;
	using UnityEngine;
	using Zenject;

	public class AbilityViewPool : Pool
	{
		private AbilityView.Factory _abilityViewFactory;

		[Inject]
		public void Initialize(AbilityView.Factory abilityViewFactory)
		{
			_abilityViewFactory = abilityViewFactory;
		}

		protected override GameObject InstantiateGameObject()
		{
			return _abilityViewFactory.Create().gameObject;
		}
	}
}