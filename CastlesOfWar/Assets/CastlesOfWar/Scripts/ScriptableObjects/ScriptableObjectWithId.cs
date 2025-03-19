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
        [ScriptableObjectId][Tooltip("Main UUID used to identify entity, guaranteed to be unique.")]
        public string Id;
        [HideInInspector] 
        public ushort SessionId; // Generated on a per-session basis using the Dictionary.  Safe for single session use, but not saving.
        
        // User
        [Tooltip("Human-readable ID, should be unique.  Never displayed in UI.")]
        public string Title;
        [Tooltip("Name that appears in UI, does not need to be unique")]
        public string DisplayName;
        [Tooltip("Description that appears in UI")]
        public string Description;
    }
}