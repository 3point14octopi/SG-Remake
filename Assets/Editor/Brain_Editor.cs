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
       
        DrawPropertiesExcluding(serializedObject, "Stats", "showStats");
        DrawStatBlock(h);

        //this is so that when we edit one of our stats using the stat blocks, it applies the changes at runtime
        EditorUtility.SetDirty(h);
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawStatBlock(Brain b)
    {
        //this lets us put the stat stuff into a dropdown, like arrays or lists
        b.showStats = EditorGUILayout.Foldout(b.showStats, b.gameObject.name.ToString() + " Stats", true);//we toggle the bool by clicking the foldout
        if(b.showStats)/*if true we draw the stat values*/
        {
            for(int i = 0; i < b.Stats.Length; i++)
            {
                EditorGUILayout.LabelField(((EntityStat)i).ToString());
                b.Stats[i] = EditorGUILayout.IntField(b.Stats[i]);
            }
        }
        
    }

}
