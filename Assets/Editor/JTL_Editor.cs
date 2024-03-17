using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JAFGridLayer)), CanEditMultipleObjects]
public class JTL_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        JAFGridLayer jgl = target as JAFGridLayer;

        serializedObject.Update();

        if (jgl.isAnimated)
        {
            DrawPropertiesExcluding(serializedObject, "staticTiles");
        }
        else
        {
            DrawPropertiesExcluding(serializedObject, "animatedTiles");
        }

        serializedObject.ApplyModifiedProperties();
    }
}
