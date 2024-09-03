using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileBlockLayer)), CanEditMultipleObjects]
public class TileBlockLayer_Editor : Editor
{
    TileBlockLayer tb;
    List<SerializedProperty> propsToDraw = new List<SerializedProperty>();

    private void OnEnable()
    {
        List<string> validProps = new List<string>()
        {
            "renderGrid",
            "originCoords",
            "anchor",
            "matrix",
            "matrixDim"
        };

        InspectorPropertyExposer pexp = new InspectorPropertyExposer();
        propsToDraw = pexp.GetSelectedProperties(serializedObject, serializedObject.targetObject.GetType(), validProps, true, propsToDraw);


        tb = target as TileBlockLayer;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();

        for (int i = 0; i < propsToDraw.Count; EditorGUILayout.PropertyField(propsToDraw[i], true), i++) ;

        if (GUILayout.Button("Recalculate"))
        {
            tb.matrix.EditDimensions(tb.matrixDim);
            
            EditorUtility.SetDirty(tb);
        }
            serializedObject.ApplyModifiedProperties();

    }
}