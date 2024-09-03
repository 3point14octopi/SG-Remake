using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TileBlockMatrix))]
public class TileBlockMatrix_Drawer : PropertyDrawer
{
    private float dHeight = 20f;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);
        Rect propertyPos = position;
        propertyPos.y += 20f;
        SerializedProperty data = property.FindPropertyRelative("rows");
        dHeight = 20f * (data.arraySize + 1);

        for(int i = 0; i < data.arraySize; i++)
        {
            SerializedProperty matrixRow = data.GetArrayElementAtIndex(i).FindPropertyRelative("elements");
            propertyPos.height = 20f;
            propertyPos.width = position.width / matrixRow.arraySize;

            for(int j = 0; j < matrixRow.arraySize; j++)
            {
                EditorGUI.PropertyField(propertyPos, matrixRow.GetArrayElementAtIndex(j), GUIContent.none, false);
                propertyPos.x += propertyPos.width;
            }

            propertyPos.x = position.x;
            propertyPos.y += 20f;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return dHeight;
    }
}
