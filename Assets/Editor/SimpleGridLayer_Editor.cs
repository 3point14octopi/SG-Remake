using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleGridLayer)), CanEditMultipleObjects]
public class SimpleGridLayer_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        SimpleGridLayer sgl = target as SimpleGridLayer;

        serializedObject.Update();
        

        if (sgl.selectionStyle == TileSelectionStyle.Shuffle)
        {
            if(sgl.odds.Length != sgl.tileList.Count)
            {
                int[] hold = sgl.odds;
                sgl.odds = new int[sgl.tileList.Count];
                for (int i = 0; i < hold.Length && i < sgl.odds.Length; sgl.odds[i] = hold[i], i++) ;
                if (sgl.odds.Length == 1) sgl.odds[0] = (int)sgl.tileRange;
            }


            // DrawPropertiesExcluding(serializedObject, "odds");
            DrawDefaultInspector();

            int sum = 0;
            for(int i = 0; i < sgl.odds.Length; i++)
            {
                EditorGUILayout.LabelField(sgl.tileList[i].name + " weight");
                sgl.odds[i] = EditorGUILayout.IntSlider(sgl.odds[i], 0, (int)sgl.tileRange - sum);
                sgl.odds[i] = (sgl.odds[i] <= sgl.tileRange - sum) ? sgl.odds[i] : 0;
                sum += sgl.odds[i];
            }
            EditorUtility.SetDirty(sgl);
        }
        else
        {
            DrawPropertiesExcluding(serializedObject, /*"odds",*/ "tileRange");
        }

        serializedObject.ApplyModifiedProperties();
    }

}


