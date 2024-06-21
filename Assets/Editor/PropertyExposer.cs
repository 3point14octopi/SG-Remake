using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class InspectorPropertyExposer
{
    public List<SerializedProperty> GetSelectedProperties(SerializedObject obj, Type targ, List<String> propertyNames, bool inclusive, List<SerializedProperty> properties)
    {
        if(targ.BaseType != typeof(ScriptableObject))
        {
            properties = GetSelectedProperties(obj, targ.BaseType, propertyNames, inclusive, properties);
        }

        IEnumerable<FieldInfo> fields = targ.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (properties == null) properties = new List<SerializedProperty>();
        foreach(FieldInfo item in fields)
        {
            if(propertyNames.Contains(item.Name) == (inclusive))
            {
                SerializedProperty prop = obj.FindProperty(item.Name);
                if (prop != null) properties.Add(prop);
            }
        }

        return properties;
    }
}