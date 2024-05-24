using UnityEditor;
using EntityStats;
using UnityEngine;

[CustomEditor(typeof(Brain)), CanEditMultipleObjects]
public class Brain_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        Brain h = target as Brain;
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "Stats");
        DrawStatBlock(h);

        EditorUtility.SetDirty(h);
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawStatBlock(Brain h)
    {
        for(int i = 0; i < h.Stats.Length; i++)
        {
            EditorGUILayout.LabelField(((EntityStat)i).ToString());
            h.Stats[i] = EditorGUILayout.IntField(h.Stats[i]);
        }
        
    }
}
