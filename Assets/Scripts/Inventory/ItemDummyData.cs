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
        }

        private void InitInventory()
        { 
            //개선 요망
            Dictionary<int, ItemVO> dict = new Dictionary<int, ItemVO>();
            Dictionary<int, ItemEntity> temp = DataManager.Instance.JsonToDict<ItemEntity>("/Data/ItemSetting.json")[0];
            foreach (var entity in temp)
            {
                dict.Add(entity.Key, entity.Value.CreateItemVo());
            }

            invenSlots[0] = dict;
        }

        private void InitInvenBar()
        {
            invenSlots[1] = new Dictionary<int, ItemVO>();
            
            invenSlots[1].Add(0,
                new ItemVO(200001, 1));

            invenSlots[1].Add(1,
                new ItemVO(203005, 1));

            invenSlots[1].Add(2,
                new ItemVO(203004, 1));
        }

        private void InitChests()
        {
            invenSlots[2] = new Dictionary<int, ItemVO>();
            invenSlots[3] = new Dictionary<int, ItemVO>();

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
            ItemDB = new ItemDB(DataManager.Instance.JsonToDict<ItemData>("/Data/ItemTable.json")[0]);
            ItemEffects = DataManager.Instance.JsonToDict<ItemEffect>("/Data/SkillTable.json")[0];
            Dictionary<int, ItemEffectRelation> effectRelation = DataManager.Instance.JsonToDict<ItemEffectRelation>("/Data/SkillTable.json")[1];
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