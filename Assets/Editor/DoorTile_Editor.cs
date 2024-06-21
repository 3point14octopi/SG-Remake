using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(DoorTile)), CanEditMultipleObjects]
public class DoorTile_Editor : Editor
{
    List<SerializedProperty> validProperties = new List<SerializedProperty>();
    DoorTile d;
    DoorStates lastState = DoorStates.OPEN;
    int quantity = 0;
    string inheritedType = "?";

    private void OnEnable()
    {
        List<string> validNames = new List<string>()
        {   
            "m_Sprite",
            "m_ColliderType",
            "m_InstancedGameObject",
            "currentState"
        };

        InspectorPropertyExposer pexp = new InspectorPropertyExposer();
        validProperties = pexp.GetSelectedProperties(serializedObject, serializedObject.targetObject.GetType(), validNames, true, validProperties);

        d = target as DoorTile;
        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawTilePreview(d.sprite);
        for (int i = 0; i < validProperties.Count(); EditorGUILayout.PropertyField(validProperties[i], true), i++);
        DrawTileList();

        EditorUtility.SetDirty(d);
        

        serializedObject.ApplyModifiedProperties();
    }


    private void DrawTilePreview(Sprite s)
    {
        //begin stolen code
        if (s == null) return;

        //get the texture of the tile
        Texture2D texture = AssetPreview.GetAssetPreview(s);
        GUILayout.Label("", GUILayout.Height(60), GUILayout.Width(60));
        //Draws the texture where we have defined our Label (empty space)
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        //end stolen code
    }

    private void DrawTileList()
    {
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        for(int i = 0; i < 4; i++)
        {
            EditorGUILayout.PrefixLabel(((DoorStates)i).ToString());
            DrawTilePreview(d.elements[i].tileSprite);
            EditorGUILayout.ObjectField(d.elements[i].tileSprite, typeof(Tile), d.elements[i].tileSprite);
            EditorGUILayout.EnumPopup(d.elements[i].tileCollider);
            



            

        }
    }
}
