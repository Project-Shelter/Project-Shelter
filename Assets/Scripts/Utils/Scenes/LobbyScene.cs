using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        SceneType = Define.Scene.Lobby;
    }

    public override void Clear()
    {
    }

    public void StartGame()
    {
        Managers.SceneManager.LoadScene(Define.Scene.Game);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
