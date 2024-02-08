using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cainos.PixelArtMonster_Dungeon
{

    [CustomEditor(typeof(MonsterController))]
    public class MonsterControllerEditor : Editor
    {
        private MonsterController instance;

        private SerializedProperty defaultMovement;
        private SerializedProperty walkSpeedMax;
        private SerializedProperty walkAcc;
        private SerializedProperty runSpeedMax;
        private SerializedProperty runAcc;
        private SerializedProperty airSpeedMax;
        private SerializedProperty airAcc;
        private SerializedProperty groundBrakeAcc;
        private SerializedProperty airBrakeAcc;
        private SerializedProperty jumpSpeed;
        private SerializedProperty jumpCooldown;
        private SerializedProperty jumpDelay;
        private SerializedProperty jumpGravityMutiplier;
        private SerializedProperty fallGravityMutiplier;

        private SerializedProperty groundCheckSize;
        private SerializedProperty movingBlendTransitionSpeed;
        private SerializedProperty canAttackInAir;
        private SerializedProperty canAttackWhenMoving;

        private SerializedProperty inputMove;
        private SerializedProperty inputMoveModifier;
        private SerializedProperty inputJump;
        private SerializedProperty inputAttack;

        private PropertyField IsDead;

        private bool foldout_parameters= true;
        private bool foldout_runtime = true;

        private void OnEnable()
        {
            instance = target as MonsterController;

            defaultMovement = serializedObject.FindProperty("defaultMovement");
            walkSpeedMax = serializedObject.FindProperty("walkSpeedMax");
            walkAcc = serializedObject.FindProperty("walkAcc");
            runSpeedMax = serializedObject.FindProperty("runSpeedMax");
            runAcc = serializedObject.FindProperty("runAcc");
            airSpeedMax = serializedObject.FindProperty("airSpeedMax");
            airAcc = serializedObject.FindProperty("airAcc");
            airSpeedMax = serializedObject.FindProperty("airSpeedMax");
            groundBrakeAcc = serializedObject.FindProperty("groundBrakeAcc");
            groundBrakeAcc = serializedObject.FindProperty("groundBrakeAcc");
            airBrakeAcc = serializedObject.FindProperty("airBrakeAcc");
            jumpSpeed = serializedObject.FindProperty("jumpSpeed");
            jumpCooldown = serializedObject.FindProperty("jumpCooldown");
            jumpDelay = serializedObject.FindProperty("jumpDelay");
            jumpGravityMutiplier = serializedObject.FindProperty("jumpGravityMutiplier");
            fallGravityMutiplier = serializedObject.FindProperty("fallGravityMutiplier");

            canAttackInAir = serializedObject.FindProperty("canAttackInAir");
            canAttackWhenMoving = serializedObject.FindProperty("canAttackWhenMoving");

            movingBlendTransitionSpeed = serializedObject.FindProperty("movingBlendTransitionSpeed");
            groundCheckSize = serializedObject.FindProperty("groundCheckSize");

            inputMove = serializedObject.FindProperty("inputMove");
            inputMoveModifier = serializedObject.FindProperty("inputMoveModifier");
            inputJump = serializedObject.FindProperty("inputJump");
            inputAttack = serializedObject.FindProperty("inputAttack");

            IsDead = ExposeProperties.GetProperty("IsDead", instance);

        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            foldout_parameters = EditorGUILayout.Foldout(foldout_parameters, "Parameters", true);
            if (foldout_parameters)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(defaultMovement);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(walkSpeedMax);
                EditorGUILayout.PropertyField(walkAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(runSpeedMax);
                EditorGUILayout.PropertyField(runAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(airSpeedMax);
                EditorGUILayout.PropertyField(airAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(groundBrakeAcc);
                EditorGUILayout.PropertyField(airBrakeAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(jumpSpeed);
                EditorGUILayout.PropertyField(jumpCooldown);
                EditorGUILayout.PropertyField(jumpDelay);
                EditorGUILayout.PropertyField(jumpGravityMutiplier);
                EditorGUILayout.PropertyField(fallGravityMutiplier);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(canAttackInAir);
                EditorGUILayout.PropertyField(canAttackWhenMoving);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(groundCheckSize);
                EditorGUILayout.PropertyField(movingBlendTransitionSpeed);
                EditorGUI.indentLevel--;
            }

            foldout_runtime = EditorGUILayout.Foldout(foldout_runtime, "Runtime", true);
            if (foldout_runtime)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(inputMove);
                EditorGUILayout.PropertyField(inputMoveModifier);
                EditorGUILayout.PropertyField(inputJump);
                EditorGUILayout.PropertyField(inputAttack);

                EditorGUILayout.Space();

                ExposeProperties.Expose(IsDead);

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
