namespace Osnowa.Osnowa.Context
{
	using System.Collections.Generic;
	using System.Linq;
	using Core.CSharpUtilities;
	using Core.ECS;
	using Core.ECS.Initiative;
	using global::Osnowa.Osnowa.Example.ECS.AI;
	using Example.ECS.Body;
	using Example.ECS.Creation;
	using Example.ECS.Identification;
	using Example.ECS.View;
	using UnityEngine;

	[CreateAssetMenu(fileName = "SavedComponents", menuName = "Osnowa/SavedComponents", order = 0)]
	public class SavedComponents : ScriptableObject, ISavedComponents, ISerializationCallbackReceiver
	{
		// meta information about component presence (due to lack of null serialization in Unity)
		public Dictionary<SGuid, bool[]> EntityToHasComponent { get; set; }
		[SerializeField] private SGuid[] _sGuids;
		[SerializeField] private bool[] _componentPresence;
		[SerializeField] private int _componentPresenceInfoPerEntity;

		[SerializeField] private SGuid _playerEntityId;
		public SGuid PlayerEntityId
		{
			get { return _playerEntityId; }
			set { _playerEntityId = value; }
		}

		// non-unique:

		[SerializeField] private IdComponent[] _ids;
		public IdComponent[] Ids
		{
			get { return _ids; }
			set { _ids = value; }
		}

		[SerializeField] private PositionComponent[] _positions;
		public PositionComponent[] Positions
		{
			get { return _positions; }
			set { _positions = value; }
		}

		[SerializeField] private VisionComponent[] _visions;
		public VisionComponent[] Visions
		{
			get { return _visions; }
			set { _visions = value; }
		}

		[SerializeField] private RecipeeComponent[] _recipees;
		public RecipeeComponent[] Recipees
		{
			get { return _recipees; }
			set { _recipees = value; }
		}

		[SerializeField] private EnergyComponent[] _energies;
		public EnergyComponent[] Energies
		{
			get { return _energies; }
			set { _energies = value; }
		}

		[SerializeField] private IntegrityComponent[] _integrities;
		public IntegrityComponent[] Integrities
		{
			get { return _integrities; }
			set { _integrities = value; }
		}

		[SerializeField] private SkillsComponent[] _skills;
		public SkillsComponent[] Skills
		{
			get { return _skills; }
			set { _skills = value; }
		}

	    [SerializeField] private StomachComponent[] _stomachs;
		public StomachComponent[] Stomachs
		{
			get { return _stomachs; }
			set { _stomachs = value; }
		}

		[SerializeField] private TeamComponent[] _teams;
		public TeamComponent[] Teams
		{
			get { return _teams; }
			set { _teams = value; }
		}

		[SerializeField] private LooksComponent[] _looks;
		public LooksComponent[] Looks
		{
			get { return _looks; }
			set { _looks = value; }
		}

		[SerializeField] private EdibleComponent[] _edibles;
		public EdibleComponent[] Edibles
		{
			get { return _edibles; }
			set { _edibles = value; }
		}

		public void OnBeforeSerialize()
		{
			if (_ids == null || _ids.Length == 0)
				return; // quinta to pomaga bo inaczej byly bledy z savedcomponents, a i tak tego nie uzywam w Quincie

			_sGuids = _ids.Select(id => id.Id).ToArray();

			_componentPresenceInfoPerEntity = EntityToHasComponent.First().Value.Length;
			_componentPresence = new bool[_componentPresenceInfoPerEntity * EntityToHasComponent.Count];
			int offset = 0;
			foreach (SGuid entityId in EntityToHasComponent.Keys)
			{
				bool[] presenceValuesForEntity = EntityToHasComponent[entityId];
				for (int presenceIndex = 0; presenceIndex < _componentPresenceInfoPerEntity; presenceIndex++)
				{
					bool presence = presenceValuesForEntity[presenceIndex];
					_componentPresence[presenceIndex + offset] = presence;
				}
				offset += _componentPresenceInfoPerEntity;
			}
		}

		public void OnAfterDeserialize()
		{
			EntityToHasComponent = new Dictionary<SGuid, bool[]>();
			int offset = 0;
			foreach (SGuid entityId in _sGuids)
			{
				var presencePerEntity = new bool[_componentPresenceInfoPerEntity];
				for (int presenceIndex = 0; presenceIndex < _componentPresenceInfoPerEntity; presenceIndex++)
				{
					presencePerEntity[presenceIndex] = _componentPresence[presenceIndex + offset];
				}
				EntityToHasComponent[entityId] = presencePerEntity;
				offset += _componentPresenceInfoPerEntity;
			}
		}
	}
}