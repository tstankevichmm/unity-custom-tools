#if UNITY_EDITOR
using CustomTools.UIExtensions;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ExtendedButton))]
public class ExtendedButton_Inspector : ButtonEditor
{
    SerializedProperty _onRightClickProperty;
    SerializedProperty _onMiddleClickProperty;

    protected override void OnEnable()
    {
        base.OnEnable();
        _onRightClickProperty = serializedObject.FindProperty("_onRightClick");
        _onMiddleClickProperty = serializedObject.FindProperty("_onMiddleClick");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // Additional code here
        
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(_onRightClickProperty);
        serializedObject.ApplyModifiedProperties();
        
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(_onMiddleClickProperty);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif