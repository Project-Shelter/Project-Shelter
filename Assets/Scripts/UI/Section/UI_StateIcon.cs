using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UI_StateIcon : UI_Section
{
    private Dictionary<string, GameObject> IconPool = new Dictionary<string, GameObject>();
    private GameObject iconPanel = null;

    enum GameObjects
    {
        IconPanel,
    }
    
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        iconPanel = GetObject((int)GameObjects.IconPanel);

        SetActor();
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.BeforeSwitchActorAction += ResyncActor;
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.SwitchActorAction += ReloadIcon;
        SetIcon();
    }
    
    //ActorStat에서 Icon을 전부 다시 받아온다.
    private void ReloadIcon()
    {
        FlushIcon();
        SetActor();
        SetIcon();
    }

    private void SetActor()
    {
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor.Buff.AddBuffAction += AddIcon;
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor.Buff.RemoveBuffAction += RemoveIcon;
    }

    private void ResyncActor()
    { 
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor.Buff.AddBuffAction -= AddIcon;
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor.Buff.RemoveBuffAction -= RemoveIcon;
    }

    //보여지는 Icon을 전부 삭제한다.
    private void FlushIcon()
    {
        /* foreach로 접근하며 요소 삭제시 예외 발생 -> 대체로 구현
        foreach(var buff in IconPool)
        {
            RemoveIcon(buff.Key);
        }
        */
        for(int i = IconPool.Count - 1; i >= 0; i--)
        {
            var item = IconPool.ElementAt(i);
            RemoveIcon(item.Key);
        }
    }

    //Actor의 Icon을 전부 생성한다.
    private void SetIcon()
    {
        foreach (var buffTag in Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor.Buff.ActivedBuffs.Keys)
        {
            AddIcon(buffTag);
        }
    }
    
    //아이콘 생성
    private void AddIcon(string buffTag)
    {
        GameObject buffIcon = Managers.UI
            .MakeSubItem<UI_Icon>(iconPanel.transform, "Icons/UI_" + buffTag + "Icon").gameObject;
        
        IconPool.TryAdd(buffTag, buffIcon);
    }
    
    //아이콘 삭제
    private void RemoveIcon(string buffTag)
    {
        if (!IconPool.ContainsKey(buffTag))
            return;
        
        Managers.Resources.Destroy(IconPool[buffTag]);
        IconPool.Remove(buffTag);
    }
}
