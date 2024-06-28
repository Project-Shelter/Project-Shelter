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
    [SerializeField] private float movingMaxOffsetY;
    [SerializeField] private float LerpTime;

    [Header("Camera Zoom")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float originSize;
    [SerializeField] private float zoomSize;
    private bool canZoom = false;

    private float movingOffsetX = 0;
    private float movingOffsetY = 0;
    private Camera cam;
    private Transform tr;
    private ActorController actorController;
    private Actor Actor { get => actorController.CurrentActor; }

    enum OffsetType
    {
        None,
        Aim,
        Moving
    }
    private OffsetType offsetType = OffsetType.None;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        tr = gameObject.transform;
    }

    private void Start()
    {
        actorController = ServiceLocator.GetService<ActorController>();
    }

    private void Update()
    {
        if (canZoom) { ZoomByInteract(); }
        else { ResetZoom(); }

        if (InputHandler.MouseSection != Direction.None)
        {
            offsetType = OffsetType.Aim;
            SetOffsetByAim();
        }
        else if (Actor.MoveBody.Velocity != Vector2.zero)
        {
            offsetType = OffsetType.Moving;
            SetOffsetByMoving();
        }
        else
        {
            ResetOffSet();
        }

        tr.position = Actor.Tr.position + new Vector3(movingOffsetX, movingOffsetY + actorOffsetY, -10);
    }

    private void ZoomByInteract()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, zoomSpeed * Time.deltaTime);
    }

    private void ResetZoom()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, originSize, zoomSpeed * Time.deltaTime);
    }

    public void SetZoom(bool canZoom)
    {
        this.canZoom = canZoom;
    }

    private void SetOffsetByAim()
    {
        Vector2 mousePosition = InputHandler.MousePosition;
        Direction direction = InputHandler.MouseSection;
        float x = mousePosition.x - Screen.width / 2;
        float y = mousePosition.y - Screen.height / 2;

        if (direction == Direction.Up)
        {
            float ratio = movingMaxOffsetY / y;
            movingOffsetX = x * ratio;
            movingOffsetY = movingMaxOffsetY;
        }
        else if (direction == Direction.Down)
        {
            float ratio = -movingMaxOffsetY / y;
            movingOffsetX = x * ratio;
            movingOffsetY = -movingMaxOffsetY;
        }
        else if (direction == Direction.Left)
        {
            float ratio = -movingMaxOffsetX / x;
            movingOffsetX = -movingMaxOffsetX;
            movingOffsetY = y * ratio;
        }
        else if (direction == Direction.Right)
        {
            float ratio = movingMaxOffsetX / x;
            movingOffsetX = movingMaxOffsetX;
            movingOffsetY = y * ratio;
        }
    }

    private void SetOffsetByMoving()
    {
        Vector2 velocity = Actor.MoveBody.Velocity;
        if (velocity.x > 0) { movingOffsetX = Mathf.Lerp(movingOffsetX, movingMaxOffsetX, LerpTime * Time.deltaTime); }
        else if (velocity.x < 0) { movingOffsetX = Mathf.Lerp(movingOffsetX, -movingMaxOffsetX, LerpTime * Time.deltaTime); }
        if (velocity.y > 0) { movingOffsetY = Mathf.Lerp(movingOffsetY, movingMaxOffsetY, LerpTime * Time.deltaTime); }
        else if (velocity.y < 0) { movingOffsetY = Mathf.Lerp(movingOffsetY, -movingMaxOffsetY, LerpTime * Time.deltaTime); }
    }

    private void ResetOffSet()
    {
        switch(offsetType)
        {
            case OffsetType.Aim:
                movingOffsetX = 0;
                movingOffsetY = 0;
                break;
            case OffsetType.Moving:
                movingOffsetX = Mathf.Lerp(movingOffsetX, 0, LerpTime * Time.deltaTime);
                movingOffsetY = Mathf.Lerp(movingOffsetY, 0, LerpTime * Time.deltaTime);
                break;
        }
    }
}
