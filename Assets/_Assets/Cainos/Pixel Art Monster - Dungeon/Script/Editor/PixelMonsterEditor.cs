using UnityEditor;
using UnityEngine;
using System.Collections;


namespace Cainos.PixelArtMonster_Dungeon
{
    [CustomEditor(typeof(PixelMonster))]
    public class PixelMonsterEditor : Editor
    {
        private PixelMonster instance;

        private SerializedProperty animator;
        private SerializedProperty renderer;
        private SerializedProperty fx;

        private SerializedProperty dieFxPrefab;

        private PropertyField Facing;
        private PropertyField AttackAction;
        private PropertyField IsHiding;
        private PropertyField IsGrounded;
        private PropertyField IsDead;
        private PropertyField MovingBlend;

        private bool foldout_objects = true;
        private bool foldout_prefabs = true;
        private bool foldout_runtime = true;

        public void OnEnable()
        {
            instance = target as PixelMonster;

            animator = serializedObject.FindProperty("animator");
            renderer = serializedObject.FindProperty("renderer");
            fx = serializedObject.FindProperty("fx");

            dieFxPrefab = serializedObject.FindProperty("dieFxPrefab");

            Facing = ExposeProperties.GetProperty("Facing", instance);
            IsGrounded = ExposeProperties.GetProperty("IsGrounded", instance);
            IsDead = ExposeProperties.GetProperty("IsDead", instance);
            IsHiding = ExposeProperties.GetProperty("IsHiding", instance);
            MovingBlend = ExposeProperties.GetProperty("MovingBlend", instance);
        }

        public override void OnInspectorGUI()
        {
            if (instance == null) return;
            serializedObject.Update();

            foldout_objects = EditorGUILayout.Foldout(foldout_objects, "Objects", true);
            if (foldout_objects)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(animator);
                EditorGUILayout.PropertyField(renderer);
                EditorGUILayout.PropertyField(fx);
                EditorGUI.indentLevel--;
            }

            foldout_prefabs = EditorGUILayout.Foldout(foldout_prefabs, "Prefabs", true);
            if (foldout_prefabs)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(dieFxPrefab);
                EditorGUI.indentLevel--;
            }

            foldout_runtime = EditorGUILayout.Foldout(foldout_runtime, "Runtime", true);
            if (foldout_runtime)
            {
                EditorGUI.indentLevel++;
                ExposeProperties.Expose(Facing);
                ExposeProperties.Expose(IsHiding);
                ExposeProperties.Expose(IsGrounded);
                ExposeProperties.Expose(IsDead);
                ExposeProperties.Expose(MovingBlend);
                EditorGUILayout.Space();

                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 10);
                if (GUILayout.Button("Injured Front")) instance.InjuredFront();
                if (GUILayout.Button("Injured Back")) instance.InjuredBack();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 10);
                if (GUILayout.Button("Attack")) instance.Attack();
                GUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
