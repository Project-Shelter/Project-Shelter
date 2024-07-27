using System.Collections;
using System.Collections.Generic;
using ItemContainer;
using UnityEngine;

public class InventorySetting : MonoBehaviour
{
    private ActorController actor;

    private Dictionary<Actor, Dictionary<int, ItemVO>>
        actorInventory = new Dictionary<Actor, Dictionary<int, ItemVO>>();
    
    private Dictionary<Actor, Dictionary<int, ItemVO>>
        actorInventoryBar = new Dictionary<Actor, Dictionary<int, ItemVO>>();
    
    void Start()
    {
        actor = ServiceLocator.GetService<ActorController>();
        
        actor.BeforeSwitchActorAction -= SaveActorInventory;
        actor.BeforeSwitchActorAction += SaveActorInventory;

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
    }
}
