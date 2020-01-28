namespace Osnowa.Osnowa.Context
{
	using System.Collections.Generic;
	using Core.CSharpUtilities;
	using global::Osnowa.Osnowa.Core.ECS;
	using global::Osnowa.Osnowa.Core.ECS.Initiative;
	using global::Osnowa.Osnowa.Example.ECS.AI;
	using global::Osnowa.Osnowa.Example.ECS.Body;
	using global::Osnowa.Osnowa.Example.ECS.Creation;
	using global::Osnowa.Osnowa.Example.ECS.Identification;
	using global::Osnowa.Osnowa.Example.ECS.View;

	public interface ISavedComponents
	{
		SGuid PlayerEntityId { get; set; }

		Dictionary<SGuid, bool[]> EntityToHasComponent { get; set; }
		
		// components:
		IdComponent[] Ids { get; set; }
		RecipeeComponent[] Recipees { get; set; }
		EnergyComponent[] Energies { get; set; }
		IntegrityComponent[] Integrities { get; set; }
		SkillsComponent[] Skills { get; set; }
	    StomachComponent[] Stomachs { get; set; }
		TeamComponent[] Teams { get; set; }
		PositionComponent[] Positions { get; set; }
		VisionComponent[] Visions { get; set; }
		LooksComponent[] Looks{ get; set; }
		EdibleComponent[] Edibles{ get; set; }
	}
}