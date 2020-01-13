using System.Collections.Generic;
using GameLogic.AI.Model;
using PCG.Recipees;
using UnityEngine;

public interface IEntityRecipee
{
	IEntityRecipee Parent { get; }
	List<IComponentRecipee> NewComponents { get; }
	List<Sprite> Sprites { get; }
	List<Skill> NewSkills { get; }
	string Id { get; }
	string Name { get; }
}