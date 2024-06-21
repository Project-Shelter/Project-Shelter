using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public DayNight DayNight { get; private set; } = new DayNight();
    public ActorController ActorController { get; private set; } = new ActorController();
    public PlayerCamera PlayerCamera { get; private set; }

    protected override void Init()
    {
        SceneType = Define.Scene.Game;
        ActorController.Init();
        PlayerCamera = Util.GetOrAddComponent<PlayerCamera>(Camera.main.gameObject);
    }

    public override void Clear()
    {

    }

    private void FixedUpdate()
    {
        DayNight.FixedUpdate();
        ActorController.FixedUpdate();
    }

    private void Update()
    {
        ActorController.Update();
    }
}