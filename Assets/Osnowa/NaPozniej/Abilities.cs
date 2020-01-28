namespace Osnowa.NaPozniej
{
	using System.Collections.Generic;
	using GameLogic.AI.Model;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Abilities", menuName = "Osnowa/Configuration/Abilities", order = 0)]
	public class Abilities : ScriptableObject
	{
		public Ability Sneak;
		public Ability DrainBlood;
		public Ability Eat;
		public Ability SpawnActors;
		public Ability PickUp;
		public Ability Drop;
		public Ability Release;
		public Ability Custom1;
		public Ability Custom2;
		public Ability Custom3;
		public Ability Custom4;
		public Ability Custom5;
		public Ability Custom6;
		public Ability Custom7;
		public Ability Custom8;
		public Ability Custom9;

		public List<Ability> AllAbilities;
	}
}