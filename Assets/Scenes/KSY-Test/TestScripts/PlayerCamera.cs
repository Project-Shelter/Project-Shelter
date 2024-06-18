using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Position")]
    [SerializeField] private float actorOffsetY;
    [SerializeField] private float movingMaxOffsetX;
    [SerializeField] private float LerpTime;

    private float movingOffsetX = 0;
    private Transform tr;

    private void Awake()
    {
        tr = gameObject.transform;
    }

    void Start()
    {
    }

    void Update()
    {
        if(InputHandler.ButtonD) movingOffsetX = Mathf.Lerp(movingOffsetX, movingMaxOffsetX, LerpTime * Time.deltaTime);
        if(InputHandler.ButtonA) movingOffsetX = Mathf.Lerp(movingOffsetX, -movingMaxOffsetX, LerpTime * Time.deltaTime);
        tr.position = Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor.Tr.position + new Vector3(movingOffsetX, actorOffsetY, -10);
    }
}
