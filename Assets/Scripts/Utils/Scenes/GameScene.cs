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
        ServiceLocator.RegisterService(ActorController);
        ServiceLocator.RegisterService(DayNight);
        ServiceLocator.RegisterService(PlayerCamera);
    }

    public override void Clear()
    {
        ServiceLocator.UnregisterService<ActorController>();
        ServiceLocator.UnregisterService<DayNight>();
        ServiceLocator.UnregisterService<PlayerCamera>();
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