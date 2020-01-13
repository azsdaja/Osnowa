namespace Editor
{
	using UnityEditor;
	using UnityEngine;
	using UnityUtilities;

	[CustomPropertyDrawer(typeof(NullableFloat))]
	public class NullableFloatDrawer : NullableDrawer { }

	[CustomPropertyDrawer(typeof(NullableInt))]
	public class NullableIntDrawer : NullableDrawer { }

	[CustomPropertyDrawer(typeof(NullableBool))]
	public class NullableBoolDrawer : NullableDrawer { }

	public class NullableDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			// The 6 comes from extra spacing between the fields (2px each)
			return Screen.width < 200 ? (16f + 18f) : 16f;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			Rect contentPosition = EditorGUI.PrefixLabel(position, label);
			if (position.height > 16f)
			{
				position.height = 16f;
				EditorGUI.indentLevel += 1;
				contentPosition = EditorGUI.IndentedRect(position);
				contentPosition.y += 18f;
			}
			contentPosition.width *= 0.5f;
			EditorGUI.indentLevel = 0;
			EditorGUIUtility.labelWidth = 24f;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("HasValue"), new GUIContent("Is?"));
			contentPosition.x += contentPosition.width;
			if (property.FindPropertyRelative("HasValue").boolValue)
			{
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("NonNullableValue"), new GUIContent("Is:"));
			}
			EditorGUI.EndProperty();
		}
	}
}