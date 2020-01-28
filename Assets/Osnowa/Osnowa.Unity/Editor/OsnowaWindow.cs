namespace Osnowa.Osnowa.Unity.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Tiles;
    using Tiles.Scripts;
    using UnityEditor;
    using UnityEngine;

    public class OsnowaWindow : EditorWindow
    {
        int ToolBarIndex;

        private GUIContent assetName;
        private GUIContent tools;
        private GUIContent about;
        private GUIContent review;

        private GUIStyle labelStyle;
        private GUIStyle PublisherNameStyle;
        private GUIStyle ToolBarStyle;
        private GUIStyle Text;
        private GUIStyle RedText;

        private List<OsnowaBaseTile> _allTiles;
        private string _info;
        private Vector2 _scrollPos;

        [MenuItem("Tools/Osnowa")]
        public static void ShowWindow()
        {
            OsnowaWindow myWindow = CreateInstance<OsnowaWindow>();
            myWindow.ShowUtility();
            myWindow.titleContent = new GUIContent("Osnowa");

            myWindow.tools = new GUIContent("Tools");
            myWindow.about = new GUIContent("About");
//            myWindow.assetName = myWindow.IconContent("<size=20><b><color=#AAAAAA> Serializable Dictionary</color></b></size>", "", "");
//            myWindow.tools = myWindow.IconContent("<size=12><b> Support</b></size>\n <size=9>Get help and talk \n with others.</size>", "_Help", "");
//            myWindow.about = myWindow.IconContent("<size=12><b> Contact</b></size>\n <size=9>Reach out and \n get help.</size>", "console.infoicon", "");
//            myWindow.review = myWindow.IconContent("<size=11><color=white> Please consider leaving a review.</color></size>", "Favorite Icon", "");

            myWindow.labelStyle = new GUIStyle(EditorStyles.label);
            myWindow.labelStyle.richText = true;

            myWindow.PublisherNameStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleLeft,
                richText = true
            };
            myWindow.ToolBarStyle = new GUIStyle("LargeButtonMid")
            {
                alignment = TextAnchor.MiddleLeft,
                richText = true
            };
            myWindow.Text = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft
            };
            myWindow.RedText = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft,
                normal = new GUIStyleState{textColor = Color.red}
            };
        }

        void OnGUI()
        {
            _scrollPos  = EditorGUILayout.BeginScrollView(_scrollPos);
            maxSize = minSize = new Vector2(300, 400);

            EditorGUILayout.Space();
            //GUILayout.Label(assetName, PublisherNameStyle);
            EditorGUILayout.Space();

            GUIContent[] toolbarOptions = new GUIContent[2];
            toolbarOptions[0] = tools;
            toolbarOptions[1] = about;

            ToolBarIndex = GUILayout.Toolbar(ToolBarIndex, toolbarOptions, ToolBarStyle, GUILayout.Height(50));

            switch (ToolBarIndex)
            {
                case 0:
                    EditorGUILayout.Space();

                    if (GUILayout.Button("Find all tiles and check validity"))
                    {
                        _allTiles = AssetLoader.LoadAll<OsnowaBaseTile>();
                        PresentTiles();
                    }
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Reset IDs of tiles that have duplicated IDs"))
                    {
                        ResetDuplicatedIds();
                    }
                    if (GUILayout.Button("Assign IDs to tiles without them"))
                    {
                        AssignMissingIds();
                    }
                    
                    EditorGUILayout.Space();
                    if (_info != null)
                    {
                        GUILayout.Label(_info, Text);
                    }
                    
                    break;

                case 1:
                    EditorGUILayout.Space();
                    EditorGUILayout.TextField("Osnowa. A roguelike framework for C#\r\n with ECS and Unity integration", Text);
                    EditorGUILayout.Space();
                    EditorGUILayout.TextField("Project website: www.github.com/azsdaja/Osnowa", Text);
                    if (GUILayout.Button("Visit"))
                    {
                        Application.OpenURL("www.github.com/azsdaja/Osnowa");
                    }
                    break;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndScrollView();
        }

        private void PresentTiles()
        {
            _info = "";
            _info += $"Found {_allTiles.Count} tiles.\r\n\r\n";
            
            if (_allTiles.Any())
            {
                bool[] idIsUsed = new bool[_allTiles.Max(t => t.Id) + 1];
                foreach (OsnowaBaseTile tile in _allTiles.OrderBy(t => t.Id))
                {
                    string labelText = $"{tile.name} with ID {tile.Id}";
                    bool isMissing = tile.Id == 0;
                    if (isMissing)
                    {
                        labelText += " - missing ID, please assign!";
                    }
                    else
                    {
                        bool isDuplicate = idIsUsed[tile.Id];
                        if (isDuplicate)
                        {
                            labelText  += " - WITH ID DUPLICATE!";
                        }
                    }

                    _info += labelText + "\r\n";
                    
                    idIsUsed[tile.Id] = true;
                }
            }
        }

        private void AssignMissingIds()
        {
            List<OsnowaBaseTile> allTiles = AssetLoader.LoadAll<OsnowaBaseTile>();
            
            bool[] takenIds = new bool[byte.MaxValue + 1];

            foreach (OsnowaBaseTile tile in allTiles)
            {
                takenIds[tile.Id] = true;
            }

            byte lastTriedId = 1;
            _info = "";
            foreach (OsnowaBaseTile declaredTile in allTiles)
            {
                if (declaredTile.Id > 0)
                    continue;
                for (byte id = lastTriedId;; id++)
                {
                    if (takenIds[id] == false)
                    {
                        declaredTile.Id = id;
                        takenIds[id] = true;
#if UNITY_EDITOR
                        EditorUtility.SetDirty(declaredTile);
#endif
                        lastTriedId = id;
                        _info += "Assigned " + id + " ID to " + declaredTile.name + " tile.\r\n";
                        break;
                    }

                    if (id == byte.MaxValue)
                        throw new InvalidOperationException("All IDs for tiles already taken.");
                }
                _info += "Remember to press ctrl+s to apply changes!";
            }

        }
        private void ResetDuplicatedIds()
        {
            List<OsnowaBaseTile> allTiles = AssetLoader.LoadAll<OsnowaBaseTile>();

            bool[] idIsUsed = new bool[_allTiles.Max(t => t.Id) + 1];
            _info = "";
            foreach (OsnowaBaseTile tile in allTiles.OrderBy(t => t.Id))
            {
                bool isDuplicate = idIsUsed[tile.Id];
                if (isDuplicate)
                {
                    tile.Id = 0;
                    EditorUtility.SetDirty(tile);
                    _info += $"Reset ID of {tile.name} to 0.\r\n";
                }

                idIsUsed[tile.Id] = true;
            }

            _info += "Remember to press ctrl+s to apply changes!";
        }
    }
}