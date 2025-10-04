using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DamageRangeAttribute))]
public class DamageRangeDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type declaringType = GetDeclaringType(property);
        object instance = GetDeclaringInstance(property);

        // Cast to BaseAttack and access the variable
        BaseAttack baseAttack = instance as BaseAttack;
        if (baseAttack != null)
        {
            int min = baseAttack.DamageRange.Min;
            int max = baseAttack.DamageRange.Max;

            property.intValue = baseAttack.DamageRange.Clamp(property.intValue);

            property.intValue = EditorGUI.IntSlider(position, label, property.intValue, min, max);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    private Type GetDeclaringType(SerializedProperty property)
    {
        object obj = property.serializedObject.targetObject;
        Type type = obj.GetType();
        string[] path = property.propertyPath.Split('.');

        // Traverse the property path to get the declaring type
        for (int i = 0; i < path.Length - 1; i++)
        {
            FieldInfo field = type.GetField(path[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
                return null;
            obj = field.GetValue(obj);
            if (obj == null)
                return null;
            type = obj.GetType();
        }
        return type;
    }

    // Helper to get the instance of the declaring type
    private object GetDeclaringInstance(SerializedProperty property)
    {
        object obj = property.serializedObject.targetObject;
        Type type = obj.GetType();
        string[] path = property.propertyPath.Split('.');

        for (int i = 0; i < path.Length - 1; i++)
        {
            FieldInfo field = type.GetField(path[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
                return null;
            obj = field.GetValue(obj);
            if (obj == null)
                return null;
            type = obj.GetType();
        }
        return obj;
    }
}
