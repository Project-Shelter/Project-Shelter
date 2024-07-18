using System.Collections.Generic;
using UnityEngine;

namespace ItemContainer
{
    public class ItemDummyData : MonoBehaviour
    {
        //정적 DB
        public static ItemDB ItemDB { get; private set; }
        public static Sprite PlainImage;

        public static ItemVO NullItem = new ItemVO();

        public const int countContainer = 10;
        public const int currentMaxWeight = 200;

        public static int currentWeight
        {
            get
            {
                int weight = 0;
                for (int i = 0; i < MaxCapacity[0]; i++)
                {
                    if (invenSlots[0].ContainsKey(i))
                    {
                        weight += invenSlots[0][i].Count * ItemDB.data[invenSlots[0][i].id].weight;
                    }
                }
                
                for (int i = 0; i < MaxCapacity[1]; i++)
                {
                    if (invenSlots[1].ContainsKey(i))
                    {
                        weight += invenSlots[1][i].Count * ItemDB.data[invenSlots[1][i].id].weight;
                    }
                }

                return weight;
            }
        }
        
        public static Dictionary<int, ItemVO>[] invenSlots = new Dictionary<int, ItemVO>[countContainer];
        public static int[] MaxCapacity = new int[countContainer];
        void Awake()
        {
            PlainImage = Managers.Resources.Load<Sprite>("Arts/Items/plain");
            Debug.Log("INIT");
            InitItemDB();
            InitInvenBar();
            InitInventory();
            InitChests();
            
            //Init 용도 - Awake 겹쳐서 따로 뺐음.
            ContainerInjector.ContainerInit();
        }

        private void InitInventory()
        {
            invenSlots[0] = new Dictionary<int, ItemVO>();
            MaxCapacity[0] = 18;
            
            invenSlots[0].Add(0,
                new ItemVO(200001, 1));
            
            invenSlots[0].Add(1,
                new ItemVO(200010, 8));
            
            invenSlots[0].Add(2,
                new ItemVO(202006, 12));
            
            invenSlots[0].Add(3,
                new ItemVO(200001, 1));
        }

        private void InitInvenBar()
        {
            invenSlots[1] = new Dictionary<int, ItemVO>();
            MaxCapacity[1] = 6;
            
            invenSlots[1].Add(0,
                new ItemVO(200001, 1));
        }

        private void InitChests()
        {
            invenSlots[2] = new Dictionary<int, ItemVO>();
            invenSlots[3] = new Dictionary<int, ItemVO>();

            MaxCapacity[2] = 12;
            MaxCapacity[3] = 12;

            invenSlots[2].Add(0,
                new ItemVO(200001, 2));
            
            invenSlots[2].Add(1,
                new ItemVO(200010, 8));
            
            invenSlots[2].Add(2,
                new ItemVO(202005, 1));
            
            invenSlots[2].Add(4,
                new ItemVO(200001, 1));
            
            invenSlots[2].Add(5,
                new ItemVO(200001, 2));
            
            invenSlots[2].Add(6,
                new ItemVO(200010, 8));
            
            invenSlots[2].Add(7,
                new ItemVO(202006, 50));
            
            invenSlots[2].Add(8,
                new ItemVO(200001, 1));
            
            invenSlots[2].Add(9,
                new ItemVO(200001, 1));
            
            invenSlots[2].Add(10,
                new ItemVO(200010, 8));
            
            invenSlots[2].Add(11,
                new ItemVO(202006, 18));
            
            invenSlots[3].Add(0,
                new ItemVO(202005, 20));
            
            invenSlots[3].Add(1,
                new ItemVO(200010, 8));
        }

        private void InitItemDB()
        {
            ItemDB = new ItemDB(DataManager.Instance.JsonToDict<ItemData>("/Data/ItemTable.json"));

            // ItemDB.data.Add(1, 
            //     new ItemData(1, "Potion", "Delicious Potion", ItemType.UseItem, 5, 1, 1, 1, 5,
            //         Managers.Resources.Load<Sprite>("Arts/Items/potion")));
            //
            // ItemDB.data.Add(2,
            //     new ItemData(2, "Bullet", "Most powerful Bullet", ItemType.UseItem, 1, 1, 1, 1, 10,
            //         Managers.Resources.Load<Sprite>("Arts/Items/bullet")));
            //
            // ItemDB.data.Add(3,
            //     new ItemData(3, "LOVE", "Mercy", ItemType.EtcItem, 0, 1, 1, 1, 1,
            //         Managers.Resources.Load<Sprite>("Arts/Items/love")));
            //
            // ItemDB.data.Add(4,
            //     new ItemData(4, "Book", "The Story of Love", ItemType.EquipItem, 20, 1, 1, 1, 3,
            //         Managers.Resources.Load<Sprite>("Arts/Items/Book")));
        }
    }
}