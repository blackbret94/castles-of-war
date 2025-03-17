using System;
using UnityEditor;
using UnityEngine;

namespace Vashta.CastlesOfWar.ScriptableObject
{
    public class ScriptableObjectIdAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
    public class ScriptableObjectIdDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUI.enabled = false;
            if (string.IsNullOrEmpty(property.stringValue)) {
                property.stringValue = Guid.NewGuid().ToString();
            }
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
    
    public class ScriptableObjectWithID : UnityEngine.ScriptableObject
    {
        // System
        [ScriptableObjectId]
        public string Id;
        [HideInInspector] 
        public ushort SessionId; // Generated on a per-session basis using the Dictionary.  Safe for single session use, but not saving.
        
        // User
        public string Title;
        public string DisplayName;
        public string Description;
    }
}