using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GunModule)), CanEditMultipleObjects]
public class Gun_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        GunModule gm = target as GunModule;
        base.OnInspectorGUI(); 
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("New Bullet", GUILayout.Width(150f))) { gm.ammoList.Add(new Ammo() + gm.currentAmmo);  }

        //Targetting style toggle
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Toggle Automatic", GUILayout.Width(150f))){gm.automatic = !gm.automatic;}
        if(gm.automatic){ EditorGUILayout.LabelField("Automatic", EditorStyles.boldLabel);}
        else if(!gm.automatic){ EditorGUILayout.LabelField("Manual", EditorStyles.boldLabel);}
        EditorGUILayout.EndHorizontal();

        //Targetting style toggle
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Switch Style", GUILayout.Width(150f))){gm.targeted = !gm.targeted;}
        if(gm.targeted){ EditorGUILayout.LabelField("Targeted", EditorStyles.boldLabel);}
        else if(!gm.targeted){ EditorGUILayout.LabelField("Preset", EditorStyles.boldLabel);}
        EditorGUILayout.EndHorizontal();

        //LOS toggle
        if(gm.targeted){
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Toggle LOS", GUILayout.Width(150f))){gm.needLOS = !gm.needLOS;}
            if(gm.needLOS){ EditorGUILayout.LabelField("LOS", EditorStyles.boldLabel);}
            else if(!gm.needLOS){ EditorGUILayout.LabelField("NO LOS", EditorStyles.boldLabel);}
            EditorGUILayout.EndHorizontal();
        }


        serializedObject.ApplyModifiedProperties();
    }

}
