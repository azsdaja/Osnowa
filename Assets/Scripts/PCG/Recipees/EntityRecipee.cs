namespace PCG.Recipees
{
	using System.Collections.Generic;
	using System.Linq;
	using GameLogic.AI.Model;
	using UnityEngine;
	using UnityUtilities;

	[CreateAssetMenu(fileName = "EntityRecipee", menuName = "Osnowa/Entities/EntityRecipee", order = 0)]
	public class EntityRecipee : ScriptableObject, IEntityRecipee
	{
		[Expandable][SerializeField] private EntityRecipee _parent;
		[Expandable][SerializeField] private List<ComponentRecipee> _newComponents;
		[SerializeField] private List<Sprite> _sprites;
		[SerializeField] private List<Skill> _newSkills;
		[SerializeField] private string _id;
		
		public IEntityRecipee Parent => _parent;

		public List<IComponentRecipee> NewComponents => _newComponents.Cast<IComponentRecipee>().ToList();

		public List<Sprite> Sprites => _sprites;

		public List<Skill> NewSkills => _newSkills;

		public string Id => _id;

        public string Name => name;
	}
}