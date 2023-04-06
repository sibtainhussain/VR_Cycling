using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(SplinePath))]
public class SplinePathEditor : Editor
{
    SplinePath source;
    SerializedProperty controlPoints;
    SerializedProperty segments;
    SerializedProperty edgeRingCount;
    SerializedProperty defaultShape2D;
    SerializedProperty defaultMaterial;
    SerializedProperty controlPointRadius;
    SerializedProperty tTest;

    void OnEnable()
    {
        source = (SplinePath)target;
        controlPoints = serializedObject.FindProperty ("controlPoints");
        segments = serializedObject.FindProperty ("segments");
        edgeRingCount = serializedObject.FindProperty ("edgeRingCount");
        defaultShape2D = serializedObject.FindProperty ("defaultShape2D");
        defaultMaterial = serializedObject.FindProperty ("defaultMaterial");
        controlPointRadius = serializedObject.FindProperty ("controlPointRadius");
        tTest = serializedObject.FindProperty ("tTest");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update ();

        EditorGUILayout.PropertyField (controlPoints, new GUIContent ("Control Points"));
        EditorGUILayout.PropertyField (segments, new GUIContent ("Segments"));
        EditorGUILayout.PropertyField (edgeRingCount, new GUIContent ("Edge Ring Count"));
        EditorGUILayout.PropertyField (defaultShape2D, new GUIContent ("Default Shape2D"));
        EditorGUILayout.PropertyField (defaultMaterial, new GUIContent ("Default Material"));
        EditorGUILayout.PropertyField (controlPointRadius, new GUIContent ("Control Point Radius"));
        EditorGUILayout.PropertyField (tTest, new GUIContent ("Test T Value"));

        if(GUILayout.Button("Add Control Point")) {
            source.AddControlPoint();
        }

        if(GUILayout.Button("Update Segments")) {
            source.CreateSegments();
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties ();
    }
}
