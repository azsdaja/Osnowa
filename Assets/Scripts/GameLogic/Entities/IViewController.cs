namespace GameLogic.Entities
{
	using Animation;
	using Osnowa.NaPozniej;
	using Osnowa.Osnowa.Entities;
	using UI;
	using UnityEngine;

	public interface IViewController : IPositionedEntity
	{
		Transform Transform { get; }
		IEntityAnimator Animator { get; }
		void HoldOnFront(GameEntity entityToHold);
		void HoldOnBack(GameEntity entityToHold);
		void DropHeldEntity(GameEntity entityToDrop);
		bool IsVisible { get; }
		void SetSprite(Sprite sprite, Color? color = null);
		IEntityUiPresenter UiPresenter { get; }
		string Name { get; set; }
		void SetAsActiveActor();
		void SetStatus(ViewStatusClass viewStatusClass, ActorStatusDefinition statusDefinition);

		/// <summary>
		/// Free the view object by for example destroying it or returning it to a pool.
		/// </summary>
		void Free();

	    Sprite GetSprite();
		void RefreshWorldPosition();
	}
}