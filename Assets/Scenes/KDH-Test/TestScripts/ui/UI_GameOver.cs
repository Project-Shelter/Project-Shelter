using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum Images
{
    GameOverPanel,
    GameOverImage,
}
enum Texts
{
    SurvivedTimeText,
}
public class UI_GameOver : UI_Scene
{
    private bool canRestart = false;

    void Start()
    {
        Init();
        Managers.Instance.GameOverAction += SetActiveGameOverUI;
        Managers.Instance.GameOverAction += SetSurvivedTimeText;
        Managers.Instance.GameOverAction += () => canRestart = true;
    }

    private void Update()
    {
        if(canRestart && InputHandler.ButtonR)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public override void Init()
    {
        base.Init();

        //바인딩
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        foreach (int image in Enum.GetValues(typeof(Images)))
        {
            GetImage(image).gameObject.SetActive(false);
        }
        foreach (int text in Enum.GetValues(typeof(Texts)))
        {
            GetText(text).gameObject.SetActive(false);
        }
    }

    private void SetActiveGameOverUI()
    {
        foreach(int image in Enum.GetValues(typeof(Images)))
        {
            GetImage(image).gameObject.SetActive(true);
        }
        foreach (int text in Enum.GetValues(typeof(Texts)))
        {
            GetText(text).gameObject.SetActive(true);
        }
    }

    private void SetSurvivedTimeText()
    {
        int day = DayNight.Instance.dayCount;
        int hour = DayNight.Instance.gameClock.Hour;
        int minute = DayNight.Instance.gameClock.Minute;
        GetText((int)Texts.SurvivedTimeText).text = day + "일 " + hour + "시 " + minute + "분까지 생존하였습니다.";
    }
}
