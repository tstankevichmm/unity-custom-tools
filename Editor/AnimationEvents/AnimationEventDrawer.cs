#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CustomTools.AnimationEvents
{
    [CustomPropertyDrawer((typeof(AnimationSignal)))]
    public class AnimationEventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty stateName = property.FindPropertyRelative("eventName");
            SerializedProperty stateEvent = property.FindPropertyRelative("OnAnimationEvent");

            Rect stateNameRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect stateEventRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2,
                position.width, EditorGUI.GetPropertyHeight(stateEvent));

            EditorGUI.PropertyField(stateNameRect, stateName);
            EditorGUI.PropertyField(stateEventRect, stateEvent, true);
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty stateEvent = property.FindPropertyRelative("OnAnimationEvent");
            float spacing = 4f;
            return EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(stateEvent) + spacing;
        }
    }
}
#endif