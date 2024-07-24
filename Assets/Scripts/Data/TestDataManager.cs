using ItemContainer;
using UnityEngine;

public class TestDataManager : MonoBehaviour
{
    void Awake()
    {
        DataManager.Instance.JsonToDict<ItemData>("/Data/ItemTable.json");
    }
}
