using System.Collections.Generic;
using UnityEngine;

namespace ItemContainer
{
    public class ItemDummyData : MonoBehaviour
    {
        //정적 DB
        public static ItemDB ItemDB { get; private set; }
        public static Dictionary<int, ItemEffect> ItemEffects{ get; private set; }
        public static Dictionary<int, List<int>> ItemEffectRelations{ get; private set; }
        public static Sprite PlainImage;

        public static ItemVO NullItem = new ItemVO();

        public const int countContainer = 1000;
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
        public static int[] MaxCapacity= {18, 6, 12, 8};
        void Awake()
        {
            PlainImage = Managers.Resources.Load<Sprite>("Arts/Items/plain");
            Debug.Log("INIT");
            InitItemDB();
            InitInvenBar();
            InitInventory();
            InitChests();
            InitTrade();
            
            //Init 용도 - Awake 겹쳐서 따로 뺐음.
            ContainerInjector.ContainerInit();
        }

        private void InitTrade()
        {
            invenSlots[700] = new Dictionary<int, ItemVO>()
            {
                { 0, new ItemVO(200001, 2) },
                { 1, new ItemVO(200010, 8) },
                { 2, new ItemVO(202005, 1) }
            };
            invenSlots[701] = new Dictionary<int, ItemVO>()
            {
                { 0, new ItemVO(200001, 2) },
                { 1, new ItemVO(200010, 8) },
                { 2, new ItemVO(202005, 1) },
                { 3, new ItemVO(200001, 1) }
            };
        }

        private void InitInventory()
        { 
            //개선 요망
            Dictionary<int, ItemVO> dict = new Dictionary<int, ItemVO>();
            Dictionary<int, ItemEntity> temp = DataManager.Instance.JsonToDict<ItemEntity>("Data/ItemSetting")[0];
            foreach (var entity in temp)
            {
                dict.Add(entity.Key, entity.Value.CreateItemVo());
            }

            invenSlots[0] = dict;
        }

        private void InitInvenBar()
        {
            invenSlots[1] = new Dictionary<int, ItemVO>();
            
        }

        private void InitChests()
        {
            for(int i = 2; i <= 40; i++)
            {
               invenSlots[i] = new Dictionary<int, ItemVO>();
            }

            invenSlots[8].Add(0,
                new ItemVO(203005, 1));

            invenSlots[2].Add(0,
                new ItemVO(200001, 1));
            invenSlots[2].Add(1,
                new ItemVO(203004, 1));

            invenSlots[13].Add(0,
                new ItemVO(202006, 30));

            List<int> excludeNum = new()
            {
                8,
                2,
                13
            };
            for (int i = 0; i < 16; i++)
            {
                int randomNum = Random.Range(2, 40);
                while (excludeNum.Contains(randomNum))
                {
                    randomNum = Random.Range(2, 40);
                }
                excludeNum.Add(randomNum);
                Debug.Log(randomNum);
                int meatNum = Random.Range(1, 2);
                invenSlots[randomNum].Add(0, new ItemVO(200001, meatNum));
            }

            for (int i = 0; i < 16; i++)
            {
                int randomNum = Random.Range(2, 40);
                while (excludeNum.Contains(randomNum))
                {
                    randomNum = Random.Range(2, 40);
                }
                excludeNum.Add(randomNum);
                Debug.Log(randomNum);
                int bulletNum = Random.Range(5, 15);
                invenSlots[randomNum].Add(0, new ItemVO(202006, bulletNum));
            }
        }

        private void InitItemDB()
        {
            ItemDB = new ItemDB(DataManager.Instance.JsonToDict<ItemData>("Data/ItemTable")[0]);
            ItemEffects = DataManager.Instance.JsonToDict<ItemEffect>("Data/SkillTable")[0];
            Dictionary<int, ItemEffectRelation> effectRelation = DataManager.Instance.JsonToDict<ItemEffectRelation>("Data/SkillTable")[1];
            ItemEffectRelations = new Dictionary<int, List<int>>();
            foreach (var data in effectRelation)
            {
                if (!ItemEffectRelations.ContainsKey(data.Value.ItemID))
                    ItemEffectRelations.Add(data.Value.ItemID, new List<int>());
                ItemEffectRelations[data.Value.ItemID].Add(data.Key);
            }
        }
    }
}