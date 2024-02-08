using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDummyData : MonoBehaviour
{
    public static List<Chest> chests = new List<Chest>();
    public static int CurrentChest = 0;
    
    private InvenItem item1;
    private InvenItem item2;
    private InvenItem item3;
    private InvenItem item4;
    void Awake()
    {
        chests = new();
        CreateData();
        GetItem();
    }

    void Start()
    {
    }

    public static void ChangeCurrentChest(int chestNum)
    {
        CurrentChest = chestNum;
    }

    void CreateData()
    {
        Inventory.Instance.DB.ItemDB.Add("Potion", new Item("Potion", "Delicious Potion", 5, 9, UsePotion));
        Inventory.Instance.DB.ItemDB.Add("Bullet", new Item("Bullet", "Most powerful Bullet", 1, 5));
        Inventory.Instance.DB.ItemDB.Add("LOVE", new Item("LOVE", "Mercy", 0, 1, UseLove));
        Inventory.Instance.DB.ItemDB.Add("Book", new Item("Book", "The Story of Love", 20, 2, UseBook));
        Inventory.Instance.DB.ItemDB.Add("Meat", new Item("Meat", "World's Most Yummy Food", 5, 10, UseMeat));

        chests.Add(new Chest());
        chests.Add(new Chest());
        chests.Add(new Chest());    
    }
    void GetItem()
    {
        /*
        Inventory.Instance.AddItem("Potion", 9);
        Inventory.Instance.AddItem("Potion", 1);
        Inventory.Instance.AddItem("Bullet", 2);
        Inventory.Instance.AddItem("LOVE", 1);
        */

        Inventory.Instance.AddItem("Book", 1);

        chests[0].AddItem("Meat", 1);
        chests[0].AddItem("Book", 1);

        chests[1].AddItem("Bullet", 5);
        chests[1].AddItem("Potion", 1);

        chests[2].AddItem("Potion", 3);
        chests[2].AddItem("Meat", 1);
    }

    // HP 30 회복
    private void UsePotion()
    {
        ActorController.Instance.CurrentActor.Stat.RestoreHP(30f);
        Debug.Log("포션을 사용했습니다. ");
    }

    // 포만감 50 회복
    private void UseMeat()
    {
        ActorController.Instance.CurrentActor.Stat.satiety.RestoreSatiety(50f);
    }
    private void UseLove()
    {
        Debug.Log("LOVE...");
    }

    private void UseBook()
    {
        Debug.Log("책을 읽었습니다. ");
    }
}
