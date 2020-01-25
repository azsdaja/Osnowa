namespace Osnowa.Osnowa.Entities
{
	using PCG.Recipees;
	using UnityEngine;

	[CreateAssetMenu(fileName = "EntityRecipees", menuName = "Osnowa/Configuration/EntityRecipees", order = 0)]
	public class EntityRecipees : ScriptableObject
	{
		public EntityRecipee Player;
		
		public EntityRecipee Deer;
		public EntityRecipee Wolf;
		public EntityRecipee Bear;
	}
}