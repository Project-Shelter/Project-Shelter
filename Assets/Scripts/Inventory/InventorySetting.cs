using System.Collections;
using System.Collections.Generic;
using ItemContainer;
using UnityEngine;

public class InventorySetting : MonoBehaviour
{
    private ActorController actor;
    private UI_InvenBar_below invenBarBelow;
    
    private Dictionary<Actor, Dictionary<int, ItemVO>>
        actorInventory = new Dictionary<Actor, Dictionary<int, ItemVO>>();
    
    private Dictionary<Actor, Dictionary<int, ItemVO>>
        actorInventoryBar = new Dictionary<Actor, Dictionary<int, ItemVO>>();
    
    void Start()
    {
        actor = ServiceLocator.GetService<ActorController>();
        //UI_InvenBar_below 가져오기 -> 추후 수정할 것(코드 결합도 ^)
        invenBarBelow = Util.FindChild(Managers.UI.Root, "UI_InvenBar_below_1", true).GetComponent<UI_InvenBar_below>();

        actor.BeforeSwitchActorAction -= SaveActorInventory;
        actor.BeforeSwitchActorAction += SaveActorInventory;
        actor.BeforeSwitchActorAction -= Managers.UI.DisableAllPopupUI;
        actor.BeforeSwitchActorAction += Managers.UI.DisableAllPopupUI;

        actor.SwitchActorAction -= ChangeActorInventory;
        actor.SwitchActorAction += ChangeActorInventory;
    }

    private void SetActorInventory()
    {
        if (actorInventory.ContainsKey(actor.CurrentActor)) return;
        actorInventory.Add(actor.CurrentActor, new Dictionary<int, ItemVO>());
        actorInventoryBar.Add(actor.CurrentActor, new Dictionary<int, ItemVO>());
    }

    private void SaveActorInventory()
    {
        SetActorInventory();
        actorInventory[actor.CurrentActor] = ItemDummyData.invenSlots[0];
        actorInventoryBar[actor.CurrentActor] = ItemDummyData.invenSlots[1];
    }

    private void ChangeActorInventory()
    {
        SetActorInventory();
        ItemDummyData.invenSlots[0] = actorInventory[actor.CurrentActor];
        ItemDummyData.invenSlots[1] = actorInventoryBar[actor.CurrentActor];
        invenBarBelow.Model.SetContainer(1);
        invenBarBelow.InitView();
    }
}
