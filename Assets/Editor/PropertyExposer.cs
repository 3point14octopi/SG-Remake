using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class InspectorPropertyExposer
{
    public List<SerializedProperty> GetSelectedProperties(SerializedObject obj, Type targ, List<String> propertyNames, bool inclusive, List<SerializedProperty> properties)
    {
        if(targ.BaseType != typeof(ScriptableObject) && targ.BaseType != typeof(MonoBehaviour))
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
                if (prop != null && !Duped(item.Name, properties)) properties.Add(prop);
            }
        }

        return properties;
    }

    private bool Duped(string name, List<SerializedProperty> props)
    {
        bool dupe = false;

        for(int i = 0; i < props.Count && dupe == false; i++)
        {
            dupe = (props[i].name == name);
        }

        return dupe;
    }
}