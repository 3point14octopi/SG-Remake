using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(AstarDebugLayer)), CanEditMultipleObjects]
public class AstarDebugLayer_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        AstarDebugLayer adl = target as AstarDebugLayer;
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "tileList", "odds", "tileRange", "selectionStyle");
        
        if (adl.enableDebuggingVisualizers)
        {
            DrawTileBoxes(adl);
            EditorUtility.SetDirty(adl);
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTileBoxes(AstarDebugLayer adl)
    {
        if (adl.tileList.Count != 3) adl.tileList = new List<TileBase>() { new Tile(), new Tile(), new Tile() };
        EditorGUILayout.LabelField("Node Tile");
        adl.tileList[0] = (TileBase)EditorGUILayout.ObjectField(adl.tileList[0], typeof(TileBase), adl.tileList[0]);
        EditorGUILayout.LabelField("Start Tile");
        adl.tileList[1] = (TileBase)EditorGUILayout.ObjectField(adl.tileList[1], typeof(TileBase), adl.tileList[1]);
        EditorGUILayout.LabelField("Goal Tile");
        adl.tileList[2] = (TileBase)EditorGUILayout.ObjectField(adl.tileList[2], typeof(TileBase), adl.tileList[2]);
    }
}
