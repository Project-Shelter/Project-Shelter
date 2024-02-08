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
        ActorController.Instance.PrevSwitchActorAction += ResyncActor;
        ActorController.Instance.SwitchActorAction += ReloadIcon;
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
        ActorController.Instance.CurrentActor.Stat.AddBuffAction += AddIcon;
        ActorController.Instance.CurrentActor.Stat.RemoveBuffAction += RemoveIcon;
    }

    private void ResyncActor()
    { 
        ActorController.Instance.CurrentActor.Stat.AddBuffAction -= AddIcon;
        ActorController.Instance.CurrentActor.Stat.RemoveBuffAction -= RemoveIcon;
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
        foreach (var buffTag in ActorController.Instance.CurrentActor.Stat.ActivedBuffs.Keys)
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
