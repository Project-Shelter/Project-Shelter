using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cainos.PixelArtMonster_Dungeon
{

    [CustomEditor(typeof(MonsterFlyingController))]
    public class MonsterFlyingControllerEditor : Editor
    {
        private MonsterFlyingController instance;

        private SerializedProperty speedMax;
        private SerializedProperty acc;
        private SerializedProperty brakeAcc;

        private SerializedProperty groundCheckSize;
        private SerializedProperty movingBlendTransitionSpeed;
        private SerializedProperty deadGravityScale;

        private SerializedProperty inputMove;
        private SerializedProperty inputAttack;

        private PropertyField IsDead;

        private bool foldout_parameter = true;
        private bool foldout_runtime = true;

        private void OnEnable()
        {
            instance = target as MonsterFlyingController;

            speedMax = serializedObject.FindProperty("speedMax");
            acc = serializedObject.FindProperty("acc");
            brakeAcc = serializedObject.FindProperty("brakeAcc");

            groundCheckSize = serializedObject.FindProperty("groundCheckSize");
            movingBlendTransitionSpeed = serializedObject.FindProperty("movingBlendTransitionSpeed");
            deadGravityScale = serializedObject.FindProperty("deadGravityScale");

            inputMove = serializedObject.FindProperty("inputMove");
            inputAttack = serializedObject.FindProperty("inputAttack");

            IsDead = ExposeProperties.GetProperty("IsDead", instance);

        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            foldout_parameter = EditorGUILayout.Foldout(foldout_parameter, "Parameter", true);
            if (foldout_parameter)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(speedMax);
                EditorGUILayout.PropertyField(acc);
                EditorGUILayout.PropertyField(brakeAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(groundCheckSize);
                EditorGUILayout.PropertyField(movingBlendTransitionSpeed);
                EditorGUILayout.PropertyField(deadGravityScale);
                EditorGUI.indentLevel--;
            }

            foldout_runtime = EditorGUILayout.Foldout(foldout_runtime, "Runtime", true);
            if (foldout_runtime)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(inputMove);
                EditorGUILayout.PropertyField(inputAttack);
                EditorGUILayout.Space();
                ExposeProperties.Expose(IsDead);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
