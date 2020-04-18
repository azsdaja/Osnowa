#if UNITY_EDITOR


using UnityEditor;
using UnityEngine;

namespace Editor.ObjectEditors
{
	using Osnowa.Osnowa.Unity.Tiles;
	using Editor = UnityEditor.Editor;

	[CustomEditor(typeof(IdGenerator))]
	public class IdGeneratorEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Write next ID to console"))
			{
				byte id = ((IdGenerator) target).GenerateId();
				Debug.Log("Next ID for a tile from tileset: " + id);
			}

			if (GUILayout.Button("Assign IDs to tileset"))
			{
				((IdGenerator) target).AssignIdsToTileset();
			}

			if (GUILayout.Button("Reset duplicate IDs"))
			{
				((IdGenerator) target).ResetDuplicateIds();
			}
		}
	}
}

#endif