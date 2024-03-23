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
        
        public static Dictionary<int, ItemVO>[] invenSlots = new Dictionary<int, ItemVO>[countContainer];
        public static int[] MaxCapacity = new int[countContainer];
        void Awake()
        {
            PlainImage = Managers.Resources.Load<Sprite>("Arts/Items/plain");

            InitItemDB();
            InitInventory();
            InitInvenBar();
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