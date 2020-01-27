namespace Osnowa.Osnowa.Unity.Tiles.Scripts.Editor
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Context;
	using Scripts;
	using UnityEditor;
	using UnityEditorInternal;
	using UnityEngine;
	using UnityEngine.Tilemaps;
	using Object = UnityEngine.Object;

	[CustomEditor(typeof(OsnowaTile))]
	[CanEditMultipleObjects]
	internal class OsnowaTileEditor : UnityEditor.Editor
	{
		private const string s_XIconString = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABoSURBVDhPnY3BDcAgDAOZhS14dP1O0x2C/LBEgiNSHvfwyZabmV0jZRUpq2zi6f0DJwdcQOEdwwDLypF0zHLMa9+NQRxkQ+ACOT2STVw/q8eY1346ZlE54sYAhVhSDrjwFymrSFnD2gTZpls2OvFUHAAAAABJRU5ErkJggg==";
		private const string s_Arrow0 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAACYSURBVDhPzZExDoQwDATzE4oU4QXXcgUFj+YxtETwgpMwXuFcwMFSRMVKKwzZcWzhiMg91jtg34XIntkre5EaT7yjjhI9pOD5Mw5k2X/DdUwFr3cQ7Pu23E/BiwXyWSOxrNqx+ewnsayam5OLBtbOGPUM/r93YZL4/dhpR/amwByGFBz170gNChA6w5bQQMqramBTgJ+Z3A58WuWejPCaHQAAAABJRU5ErkJggg==";
		private const string s_Arrow1 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABqSURBVDhPxYzBDYAgEATpxYcd+PVr0fZ2siZrjmMhFz6STIiDs8XMlpEyi5RkO/d66TcgJUB43JfNBqRkSEYDnYjhbKD5GIUkDqRDwoH3+NgTAw+bL/aoOP4DOgH+iwECEt+IlFmkzGHlAYKAWF9R8zUnAAAAAElFTkSuQmCC";
		private const string s_Arrow2 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAAC0SURBVDhPjVE5EsIwDMxPKFKYF9CagoJH8xhaMskLmEGsjOSRkBzYmU2s9a58TUQUmCH1BWEHweuKP+D8tphrWcAHuIGrjPnPNY8X2+DzEWE+FzrdrkNyg2YGNNfRGlyOaZDJOxBrDhgOowaYW8UW0Vau5ZkFmXbbDr+CzOHKmLinAXMEePyZ9dZkZR+s5QX2O8DY3zZ/sgYcdDqeEVp8516o0QQV1qeMwg6C91toYoLoo+kNt/tpKQEVvFQAAAAASUVORK5CYII=";
		private const string s_Arrow3 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAAB2SURBVDhPzY1LCoAwEEPnLi48gW5d6p31bH5SMhp0Cq0g+CCLxrzRPqMZ2pRqKG4IqzJc7JepTlbRZXYpWTg4RZE1XAso8VHFKNhQuTjKtZvHUNCEMogO4K3BhvMn9wP4EzoPZ3n0AGTW5fiBVzLAAYTP32C2Ay3agtu9V/9PAAAAAElFTkSuQmCC";
		private const string s_Arrow5 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABqSURBVDhPnY3BCYBADASvFx924NevRdvbyoLBmNuDJQMDGjNxAFhK1DyUQ9fvobCdO+j7+sOKj/uSB+xYHZAxl7IR1wNTXJeVcaAVU+614uWfCT9mVUhknMlxDokd15BYsQrJFHeUQ0+MB5ErsPi/6hO1AAAAAElFTkSuQmCC";
		private const string s_Arrow6 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAACaSURBVDhPxZExEkAwEEVzE4UiTqClUDi0w2hlOIEZsV82xCZmQuPPfFn8t1mirLWf7S5flQOXjd64vCuEKWTKVt+6AayH3tIa7yLg6Qh2FcKFB72jBgJeziA1CMHzeaNHjkfwnAK86f3KUafU2ClHIJSzs/8HHLv09M3SaMCxS7ljw/IYJWzQABOQZ66x4h614ahTCL/WT7BSO51b5Z5hSx88AAAAAElFTkSuQmCC";
		private const string s_Arrow7 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABQSURBVDhPYxh8QNle/T8U/4MKEQdAmsz2eICx6W530gygr2aQBmSMphkZYxqErAEXxusKfAYQ7XyyNMIAsgEkaYQBkAFkaYQBsjXSGDAwAAD193z4luKPrAAAAABJRU5ErkJggg==";
		private const string s_Arrow8 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAACYSURBVDhPxZE9DoAwCIW9iUOHegJXHRw8tIdx1egJTMSHAeMPaHSR5KVQ+KCkCRF91mdz4VDEWVzXTBgg5U1N5wahjHzXS3iFFVRxAygNVaZxJ6VHGIl2D6oUXP0ijlJuTp724FnID1Lq7uw2QM5+thoKth0N+GGyA7IA3+yM77Ag1e2zkey5gCdAg/h8csy+/89v7E+YkgUntOWeVt2SfAAAAABJRU5ErkJggg==";
		private const string s_MirrorX = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwQAADsEBuJFr7QAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC41ZYUyZQAAAG1JREFUOE+lj9ENwCAIRB2IFdyRfRiuDSaXAF4MrR9P5eRhHGb2Gxp2oaEjIovTXSrAnPNx6hlgyCZ7o6omOdYOldGIZhAziEmOTSfigLV0RYAB9y9f/7kO8L3WUaQyhCgz0dmCL9CwCw172HgBeyG6oloC8fAAAAAASUVORK5CYII=";
		private const string s_MirrorY = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC41ZYUyZQAAAG9JREFUOE+djckNACEMAykoLdAjHbPyw1IOJ0L7mAejjFlm9hspyd77Kk+kBAjPOXcakJIh6QaKyOE0EB5dSPJAiUmOiL8PMVGxugsP/0OOib8vsY8yYwy6gRyC8CB5QIWgCMKBLgRSkikEUr5h6wOPWfMoCYILdgAAAABJRU5ErkJggg==";
		private const string s_Rotated = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwQAADsEBuJFr7QAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC41ZYUyZQAAAHdJREFUOE+djssNwCAMQxmIFdgx+2S4Vj4YxWlQgcOT8nuG5u5C732Sd3lfLlmPMR4QhXgrTQaimUlA3EtD+CJlBuQ7aUAUMjEAv9gWCQNEPhHJUkYfZ1kEpcxDzioRzGIlr0Qwi0r+Q5rTgM+AAVcygHgt7+HtBZs/2QVWP8ahAAAAAElFTkSuQmCC";
		
		private static Texture2D[] s_Arrows;
		public static Texture2D[] arrows
		{
			get
			{
				if (s_Arrows == null)
				{
					s_Arrows = new Texture2D[10];
					s_Arrows[0] = Base64ToTexture(s_Arrow0);
					s_Arrows[1] = Base64ToTexture(s_Arrow1);
					s_Arrows[2] = Base64ToTexture(s_Arrow2);
					s_Arrows[3] = Base64ToTexture(s_Arrow3);
					s_Arrows[5] = Base64ToTexture(s_Arrow5);
					s_Arrows[6] = Base64ToTexture(s_Arrow6);
					s_Arrows[7] = Base64ToTexture(s_Arrow7);
					s_Arrows[8] = Base64ToTexture(s_Arrow8);
					s_Arrows[9] = Base64ToTexture(s_XIconString);
				}
				return s_Arrows;
			}
		}

		private static Texture2D[] s_AutoTransforms;
		public static Texture2D[] autoTransforms
		{
			get
			{
				if (s_AutoTransforms == null)
				{
					s_AutoTransforms = new Texture2D[3];
					s_AutoTransforms[0] = Base64ToTexture(s_Rotated);
					s_AutoTransforms[1] = Base64ToTexture(s_MirrorX);
					s_AutoTransforms[2] = Base64ToTexture(s_MirrorY);
				}
				return s_AutoTransforms;
			}
		}
		
		private ReorderableList m_ReorderableList;
		public OsnowaTile EditedTile => target as OsnowaTile;

		const float k_DefaultElementHeight = 48f;
		const float k_PaddingBetweenRules = 13f;
		const float k_SingleLineHeight = 16f;
		const float k_LabelWidth = 53f;
			
		public void OnEnable()
		{
			if (EditedTile.m_TilingRules == null)
				EditedTile.m_TilingRules = new List<OsnowaTile.TilingRule>();

			m_ReorderableList = new ReorderableList(EditedTile.m_TilingRules, typeof(OsnowaTile.TilingRule), true, true, true, true);
			m_ReorderableList.drawHeaderCallback = OnDrawHeader;
			m_ReorderableList.drawElementCallback = OnDrawElement;
			m_ReorderableList.elementHeightCallback = GetElementHeight;
			m_ReorderableList.onReorderCallback = ListUpdated;
			
		}

		private void ListUpdated(ReorderableList list)
		{
			SaveTile();
		}

		private float GetElementHeight(int index)
		{
			if (EditedTile.m_TilingRules != null && EditedTile.m_TilingRules.Count > 0)
			{
				switch (EditedTile.m_TilingRules[index].m_Output)
				{
					case OsnowaTile.TilingRule.OutputSprite.Random:
						return k_DefaultElementHeight + k_SingleLineHeight*(EditedTile.m_TilingRules[index].m_Sprites.Length + 3) + k_PaddingBetweenRules;
					case OsnowaTile.TilingRule.OutputSprite.Animation:
						return k_DefaultElementHeight + k_SingleLineHeight*(EditedTile.m_TilingRules[index].m_Sprites.Length + 2) + k_PaddingBetweenRules;
				}
			}
			return k_DefaultElementHeight + k_PaddingBetweenRules;
		}

		private void OnDrawElement(Rect rect, int index, bool isactive, bool isfocused)
		{
			OsnowaTile.TilingRule rule = EditedTile.m_TilingRules[index];

			float yPos = rect.yMin + 2f;
			float height = rect.height - k_PaddingBetweenRules;
			float matrixWidth = k_DefaultElementHeight;
			
			Rect inspectorRect = new Rect(rect.xMin, yPos, rect.width - matrixWidth * 2f - 20f, height);
			Rect matrixRect = new Rect(rect.xMax - matrixWidth * 2f - 10f, yPos, matrixWidth, k_DefaultElementHeight);
			Rect spriteRect = new Rect(rect.xMax - matrixWidth - 5f, yPos, matrixWidth, k_DefaultElementHeight);

			EditorGUI.BeginChangeCheck();
			RuleInspectorOnGUI(inspectorRect, rule);
			RuleMatrixOnGUI(matrixRect, rule);
			SpriteOnGUI(spriteRect, rule);
			if (EditorGUI.EndChangeCheck())
				SaveTile();
		}

		private void SaveTile()
		{
			Undo.RecordObject(this, "saved Osnowa tile");
			EditorUtility.SetDirty(target);
			Repaint();
			SceneView.RepaintAll();
		}

		private void OnDrawHeader(Rect rect)
		{
			GUI.Label(rect, "Tiling Rules");
		}

		public override void OnInspectorGUI()
		{
			int idFromEditor = EditorGUILayout.IntField("Unique ID", EditedTile.Id);
			if (idFromEditor != EditedTile.Id)
			{
				EditedTile.Id = (byte) idFromEditor;
				SaveTile();
			}

			TilemapLayer layerFromEditor = (TilemapLayer)EditorGUILayout.EnumPopup("Layer", EditedTile.Layer);
			if (layerFromEditor != EditedTile.Layer)
			{
				EditedTile.Layer = layerFromEditor;
				SaveTile();
			}

			WalkabilityModifier walkabilityFromEditor = (WalkabilityModifier) EditorGUILayout.EnumPopup("Walkability", EditedTile.Walkability);
			if (walkabilityFromEditor != EditedTile.Walkability)
			{
				EditedTile.Walkability = walkabilityFromEditor;
				SaveTile();
			}

			PassingLightModifier passingLightFromEditor = (PassingLightModifier) EditorGUILayout.EnumPopup("Passing light", EditedTile.IsPassingLight);
			if (passingLightFromEditor != EditedTile.IsPassingLight)
			{
				EditedTile.IsPassingLight = passingLightFromEditor;
				SaveTile();
			}

			EditorGUILayout.LabelField("Consistency class : when more than 0, the tile is treated by other OsnowaTiles as \"Neighbor.This\" for all other tiles with same consistency class.",
				new GUIStyle{wordWrap = true});
			int consistencyClassFromEditor = EditorGUILayout.IntField("Consistency class", EditedTile.ConsistencyClass);
			if (consistencyClassFromEditor != EditedTile.ConsistencyClass)
			{
				EditedTile.ConsistencyClass = consistencyClassFromEditor;
				SaveTile();
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Rule generation. \r\n" +
			                           "1. Drag in a prepared multi-sprite, \r\n" +
			                           "2. generate the rules (<b>in order to see them you need to change Inspector focus to something else and back to this tile</b>),\r\n" +
			                           "3. <b>remember to save (ctrl+s) afterwards</b>.", new GUIStyle{wordWrap = true});
			EditedTile.GenerateFrom = (Sprite) EditorGUILayout.ObjectField("Multi-sprite source:", EditedTile.GenerateFrom, typeof(Sprite), false);

			if (GUILayout.Button("Remove all rules"))
			{
				EditedTile.m_TilingRules = null;
				SaveTile();
			}


			if (EditedTile.GenerateFrom != null && GUILayout.Button("Generate rules (4-sided neighbourhood)"))
			{
				int[] sides = 
				{
					236,     41236,    412,    2,   
					89632,   78946123, 87412,  82,  
					896,     47896,    478,    8,   
					6,       46,       4,      5,   
				};
				
				GenerateRules(sides, 5);
			}

			if (EditedTile.GenerateFrom != null && GUILayout.Button("Generate rules (8-sided neighbourhood)"))
			{
				// Performance tip if filling the tilemap is to slow:
				// Get occurence statistics of different rules (for sparsely- and densely appearing tiles)
				// and adjust the order of rule creation for them. That would help because refreshing of single tile
				// sequentially goes through all the rules untill it meets the satisfying one.
				int[] sides = 
				{
					236,     41236,    412,    2,    0,     42,     426,   26,
					89632,   78946123, 87412,  82,   478263, 826,    4826,  842,
					896,     47896,    478,    8,    896412, 86,     486,   48,
					6,       46,       4,      5,    489632, 478962, 4236,  841236,
					6987412, 4789632,  2146,   4782, 6982,   84269,  86247, 874126,
					
					8741236,  8963214, 8632,   8964, 8746,   8214,   86214, 84236
				};
				
				GenerateRules(sides, 8);
			}

/* Doesn't work :( ctrl+s is needed anyway.
			if (GUILayout.Button("Force save"))
			{
				SaveTile();
			}*/

			Sprite defaultSpriteFromEditor = EditorGUILayout.ObjectField("Default Sprite", EditedTile.m_DefaultSprite, typeof(Sprite), false) as Sprite;
			if (defaultSpriteFromEditor != EditedTile.m_DefaultSprite)
			{
				EditedTile.m_DefaultSprite = defaultSpriteFromEditor;
				SaveTile();
			}

			EditorGUILayout.LabelField("Drag another tile below if you want to use it as a shorter variant to display eg. when player is close to the tile:", new GUIStyle{wordWrap = true});
			OsnowaBaseTile shorterVariantTileFromEditor =
				EditorGUILayout.ObjectField("Shorter variant", EditedTile.ShorterVariant, typeof(OsnowaBaseTile), false) as OsnowaBaseTile;
			if (shorterVariantTileFromEditor != EditedTile.ShorterVariant)
			{
				EditedTile.ShorterVariant = shorterVariantTileFromEditor;
				SaveTile();
			}

			var defaultColliderTypeFromEditor = (Tile.ColliderType) EditorGUILayout.EnumPopup("Default Collider", EditedTile.m_DefaultColliderType);
			if (defaultColliderTypeFromEditor != EditedTile.m_DefaultColliderType)
			{
				EditedTile.m_DefaultColliderType = defaultColliderTypeFromEditor;
				SaveTile();
			}

			EditorGUILayout.Space();

			if (m_ReorderableList != null && EditedTile.m_TilingRules != null)
				m_ReorderableList.DoLayoutList();
		}

		/// <param name="centralRuleIndex">Index in array of the sprite where all neighbours are present (78946123) minus one because there is no rule for skipped '0' element</param>
		private void GenerateRules(int[] sidesForRules, int centralRuleIndex)
		{
			EditedTile.m_TilingRules = new List<OsnowaTile.TilingRule>();

			// order of arrows in rule tile editor (order in neighbours array starting from 0):
			// 012
			// 3 4
			// 567

			string spriteParentPath = AssetDatabase.GetAssetPath(EditedTile.GenerateFrom);
			Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteParentPath).OfType<Sprite>().ToArray();

			if (sprites.Length < sidesForRules.Length)
			{
				Debug.LogError("not enough sprites. Sprites.Length = " + sprites.Length + " but should be at least " + sidesForRules.Length);
				return;
			}

			// additional central sprites are alternative versions of central tile that should be present after all sides variant sprites
			int amountOfAdditionalCentralSprites = sprites.Length - sidesForRules.Length;

			// sprites for all neighbourhood situations
			for (int i = 0; i < sidesForRules.Length; i++)
			{
				AddRuleForSprite(sidesForRules, i, sprites, amountOfAdditionalCentralSprites);
			}

			int firstIndexForAdditionalSprites = sidesForRules.Length;
			for (int currentAdditionalSpriteIndex = 0; currentAdditionalSpriteIndex < amountOfAdditionalCentralSprites; currentAdditionalSpriteIndex++)
			{
				AddToRandomRuleForCenter(currentAdditionalSpriteIndex, sprites[firstIndexForAdditionalSprites + currentAdditionalSpriteIndex],
					EditedTile.m_TilingRules[centralRuleIndex]);
			}

			SaveTile();
		}

		private void AddRuleForSprite(int[] sides, int i, Sprite[] sprites, int amountOfAdditionalCentralSprites)
		{
			int matchingPattern = sides[i];
			if (matchingPattern == 0)
				return;
			OsnowaTile.TilingRule rule = new OsnowaTile.TilingRule {m_Sprites = {[0] = sprites[i]}};
			char[] patternDigits = matchingPattern.ToString().ToCharArray();

			string centralSpriteSides = "78946123";
			bool isCentral = centralSpriteSides.All(d => patternDigits.Contains(d));
			if (isCentral)
			{
				EditedTile.m_DefaultSprite = sprites[i];
				rule.m_Sprites = new Sprite[1 + amountOfAdditionalCentralSprites];
				rule.m_Sprites[0] = sprites[i];
				if (amountOfAdditionalCentralSprites > 0)
				{
					rule.m_Output = OsnowaTile.TilingRule.OutputSprite.Random;
					rule.m_RandomTransform = OsnowaTile.TilingRule.Transform.MirrorX;
				}
			}

			SetNeighborForCornerIfNeeded(patternDigits, '7', 0, '4', '8', rule);
			SetNeighborForCornerIfNeeded(patternDigits, '9', 2, '6', '8', rule);
			SetNeighborForCornerIfNeeded(patternDigits, '1', 5, '4', '2', rule);
			SetNeighborForCornerIfNeeded(patternDigits, '3', 7, '2', '6', rule);

			rule.m_Neighbors[1] = patternDigits.Contains('8')
				? OsnowaTile.TilingRule.Neighbor.This
				: OsnowaTile.TilingRule.Neighbor.NotThis;
			rule.m_Neighbors[3] = patternDigits.Contains('4')
				? OsnowaTile.TilingRule.Neighbor.This
				: OsnowaTile.TilingRule.Neighbor.NotThis;
			rule.m_Neighbors[4] = patternDigits.Contains('6')
				? OsnowaTile.TilingRule.Neighbor.This
				: OsnowaTile.TilingRule.Neighbor.NotThis;
			rule.m_Neighbors[6] = patternDigits.Contains('2')
				? OsnowaTile.TilingRule.Neighbor.This
				: OsnowaTile.TilingRule.Neighbor.NotThis;

			EditedTile.m_TilingRules.Add(rule);
		}

		private void AddToRandomRuleForCenter(int additionalSpriteIndex, Sprite sprite, OsnowaTile.TilingRule randomRuleForCenter)
		{
			randomRuleForCenter.m_Sprites[1 + additionalSpriteIndex] = sprite;
		}

		private static void SetNeighborForCornerIfNeeded(char[] patternDigits, char probed, int probedIndex, char firstNeeded, char secondNeeded, OsnowaTile.TilingRule rule)
		{
			if (patternDigits.Contains(firstNeeded) && patternDigits.Contains(secondNeeded))
				rule.m_Neighbors[probedIndex] = patternDigits.Contains(probed)
					? OsnowaTile.TilingRule.Neighbor.This
					: OsnowaTile.TilingRule.Neighbor.NotThis;
		}

		private static void RuleMatrixOnGUI(Rect rect, OsnowaTile.TilingRule tilingRule)
		{
			Handles.color = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.2f) : new Color(0f, 0f, 0f, 0.2f);
			int index = 0;
			float w = rect.width / 3f;
			float h = rect.height / 3f;

			for (int y = 0; y <= 3; y++)
			{
				float top = rect.yMin + y * h;
				Handles.DrawLine(new Vector3(rect.xMin, top), new Vector3(rect.xMax, top));
			}
			for (int x = 0; x <= 3; x++)
			{
				float left = rect.xMin + x * w;
				Handles.DrawLine(new Vector3(left, rect.yMin), new Vector3(left, rect.yMax));
			}
			Handles.color = Color.white;

			for (int y = 0; y <= 2; y++)
			{
				for (int x = 0; x <= 2; x++)
				{
					Rect r = new Rect(rect.xMin + x * w, rect.yMin + y * h, w - 1, h - 1);
					if (x != 1 || y != 1)
					{
						switch (tilingRule.m_Neighbors[index])
						{
							case OsnowaTile.TilingRule.Neighbor.This:
								GUI.DrawTexture(r, arrows[y*3 + x]);
								break;
							case OsnowaTile.TilingRule.Neighbor.NotThis:
								GUI.DrawTexture(r, arrows[9]);
								break;
						}
						if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
						{
							tilingRule.m_Neighbors[index] = (OsnowaTile.TilingRule.Neighbor) (((int)tilingRule.m_Neighbors[index] + 1) % 3);
							GUI.changed = true;
							Event.current.Use();
						}

						index++;
					}
					else
					{
						switch (tilingRule.m_RuleTransform)
						{
							case OsnowaTile.TilingRule.Transform.Rotated:
								GUI.DrawTexture(r, autoTransforms[0]);
								break;
							case OsnowaTile.TilingRule.Transform.MirrorX:
								GUI.DrawTexture(r, autoTransforms[1]);
								break;
							case OsnowaTile.TilingRule.Transform.MirrorY:
								GUI.DrawTexture(r, autoTransforms[2]);
								break;
						}

						if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
						{
							tilingRule.m_RuleTransform = (OsnowaTile.TilingRule.Transform)(((int)tilingRule.m_RuleTransform + 1) % 4);
							GUI.changed = true;
							Event.current.Use();
						}
					}
				}
			}
		}

		private static void OnSelect(object userdata)
		{
			MenuItemData data = (MenuItemData) userdata;
			data.m_Rule.m_RuleTransform = data.m_NewValue;
		}

		private class MenuItemData
		{
			public OsnowaTile.TilingRule m_Rule;
			public OsnowaTile.TilingRule.Transform m_NewValue;

			public MenuItemData(OsnowaTile.TilingRule mRule, OsnowaTile.TilingRule.Transform mNewValue)
			{
				this.m_Rule = mRule;
				this.m_NewValue = mNewValue;
			}
		}

		private void SpriteOnGUI(Rect rect, OsnowaTile.TilingRule tilingRule)
		{
			tilingRule.m_Sprites[0] = EditorGUI.ObjectField(new Rect(rect.xMax - rect.height, rect.yMin, rect.height, rect.height), tilingRule.m_Sprites[0], typeof (Sprite), false) as Sprite;
		}

		private static void RuleInspectorOnGUI(Rect rect, OsnowaTile.TilingRule tilingRule)
		{
			float y = rect.yMin;
			EditorGUI.BeginChangeCheck();
			GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Rule");
			tilingRule.m_RuleTransform = (OsnowaTile.TilingRule.Transform)EditorGUI.EnumPopup(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_RuleTransform);
			y += k_SingleLineHeight;
			GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Collider");
			tilingRule.m_ColliderType = (Tile.ColliderType)EditorGUI.EnumPopup(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_ColliderType);
			y += k_SingleLineHeight;
			GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Output");
			tilingRule.m_Output = (OsnowaTile.TilingRule.OutputSprite)EditorGUI.EnumPopup(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_Output);
			y += k_SingleLineHeight;

			if (tilingRule.m_Output == OsnowaTile.TilingRule.OutputSprite.Animation)
			{
				GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Speed");
				tilingRule.m_AnimationSpeed = EditorGUI.FloatField(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_AnimationSpeed);
				y += k_SingleLineHeight;
			}
			if (tilingRule.m_Output == OsnowaTile.TilingRule.OutputSprite.Random)
			{
				GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Noise");
				tilingRule.m_PerlinScale = EditorGUI.Slider(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_PerlinScale, 0.001f, 0.999f);
				y += k_SingleLineHeight;

				GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Shuffle");
				tilingRule.m_RandomTransform = (OsnowaTile.TilingRule.Transform)EditorGUI.EnumPopup(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_RandomTransform);
				y += k_SingleLineHeight;
			}

			if (tilingRule.m_Output != OsnowaTile.TilingRule.OutputSprite.Single)
			{
				GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Size");
				EditorGUI.BeginChangeCheck();
				int newLength = EditorGUI.DelayedIntField(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_Sprites.Length);
				if (EditorGUI.EndChangeCheck())
					Array.Resize(ref tilingRule.m_Sprites, Math.Max(newLength, 1));
				y += k_SingleLineHeight;

				for (int i = 0; i < tilingRule.m_Sprites.Length; i++)
				{
					tilingRule.m_Sprites[i] = EditorGUI.ObjectField(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_Sprites[i], typeof(Sprite), false) as Sprite;
					y += k_SingleLineHeight;
				}
			}
		}

		public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
		{
			if (EditedTile.m_DefaultSprite != null)
			{
				Type t = GetType("UnityEditor.SpriteUtility");
				if (t != null)
				{
					MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] {typeof (Sprite), typeof (Color), typeof (int), typeof (int)});
					if (method != null)
					{
						object ret = method.Invoke("RenderStaticPreview", new object[] {EditedTile.m_DefaultSprite, Color.white, width, height});
						if (ret is Texture2D)
							return ret as Texture2D;
					}
				}
			}
			return base.RenderStaticPreview(assetPath, subAssets, width, height);
		}

		private static Type GetType(string TypeName)
		{
			var type = Type.GetType(TypeName);
			if (type != null)
				return type;

			if (TypeName.Contains("."))
			{
				var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));
				var assembly = Assembly.Load(assemblyName);
				if (assembly == null)
					return null;
				type = assembly.GetType(TypeName);
				if (type != null)
					return type;
			}

			var currentAssembly = Assembly.GetExecutingAssembly();
			var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
			foreach (var assemblyName in referencedAssemblies)
			{
				var assembly = Assembly.Load(assemblyName);
				if (assembly != null)
				{
					type = assembly.GetType(TypeName);
					if (type != null)
						return type;
				}
			}
			return null;
		}

		private static Texture2D Base64ToTexture(string base64)
		{
			Texture2D t = new Texture2D(1, 1);
			t.hideFlags = HideFlags.HideAndDontSave;
			t.LoadImage(System.Convert.FromBase64String(base64));
			return t;
		}
	}
}
