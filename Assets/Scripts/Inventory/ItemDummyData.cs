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

            InitItemDB();
            InitInvenBar();
            InitInventory();
            InitChests();
        }

        private void InitInventory()
        {
            invenSlots[0] = new Dictionary<int, ItemVO>();
            MaxCapacity[0] = 18;
            
            invenSlots[0].Add(0,
                new ItemVO(1, 5));
            
            invenSlots[0].Add(1,
                new ItemVO(2, 8));
            
            invenSlots[0].Add(2,
                new ItemVO(3, 1));
            
            invenSlots[0].Add(3,
                new ItemVO(1, 3));
        }

        private void InitInvenBar()
        {
            invenSlots[1] = new Dictionary<int, ItemVO>();
            MaxCapacity[1] = 6;
            
            invenSlots[1].Add(0,
                new ItemVO(1, 5));
        }

        private void InitChests()
        {
            invenSlots[2] = new Dictionary<int, ItemVO>();
            invenSlots[3] = new Dictionary<int, ItemVO>();

            MaxCapacity[2] = 12;
            MaxCapacity[3] = 12;

            invenSlots[2].Add(0,
                new ItemVO(1, 5));
            
            invenSlots[2].Add(1,
                new ItemVO(2, 8));
            
            invenSlots[2].Add(2,
                new ItemVO(3, 1));
            
            invenSlots[2].Add(4,
                new ItemVO(1, 3));
            
            invenSlots[2].Add(5,
                new ItemVO(1, 5));
            
            invenSlots[2].Add(6,
                new ItemVO(2, 8));
            
            invenSlots[2].Add(7,
                new ItemVO(3, 1));
            
            invenSlots[2].Add(8,
                new ItemVO(1, 3));
            
            invenSlots[2].Add(9,
                new ItemVO(1, 5));
            
            invenSlots[2].Add(10,
                new ItemVO(2, 8));
            
            invenSlots[2].Add(11,
                new ItemVO(3, 1));
            
            invenSlots[3].Add(0,
                new ItemVO(1, 5));
            
            invenSlots[3].Add(1,
                new ItemVO(2, 8));
        }

        private void InitItemDB()
        {
            ItemDB = new ItemDB();
            
            ItemDB.data.Add(1, 
                new ItemData(1, 5, 5,
                    "Potion", "Delicious Potion",
                    Managers.Resources.Load<Sprite>("Arts/Items/Potion")));
            
            ItemDB.data.Add(2,
                new ItemData(2, 1, 10,
                    "Bullet", "Most powerful Bullet", 
                    Managers.Resources.Load<Sprite>("Arts/Items/Bullet")));

            ItemDB.data.Add(3,
                new ItemData(3, 0, 1,
                    "LOVE", "Mercy",
                    Managers.Resources.Load<Sprite>("Arts/Items/Love")));

            ItemDB.data.Add(4,
                new ItemData(4, 20, 3,
                    "Book", "The Story of Love",
                    Managers.Resources.Load<Sprite>("Arts/Items/Book")));
        }
    }
}