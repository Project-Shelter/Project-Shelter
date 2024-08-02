using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public enum ItemType{
    UseItem,
    EtcItem,
    EquipItem,
    ActionItem,
}

public enum EffectType
{
    Heal,
    HealHunger,
    HealThirsty,
    HealInfection,
    Damage,
}


//정적 데이터: 마지막에 Data
//동적 데이터: 마지막에 VO
namespace ItemContainer{
    
    //UI에 보여짐/인벤토리 DB에 저장되는 아이템
    public class ItemVO
    {
        public int id;

        //필요하다면 여기서 범위 체크
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if (id is not 0 && ItemDummyData.ItemDB.data[id].overlapCount < count)
                {
                    return;
                }
                count = value;
                OnCountChanged?.Invoke(value);
            }
        }
        public Action<int> OnCountChanged;
        public ItemVO(int id, int count)
        {
            this.id = id;
            this.Count = count;
        }
        
        public ItemVO()
        {
            id = 0;
            Count = -1;
        }
    }

    public class ItemEntity : DBData
    {
        [JsonProperty("Item_ID")]
        public int item_id;
        [JsonProperty("Item_Count")]
        public int count;
        
        public ItemEntity(int id, int itemId, int count)
        {
            ID = id;
            this.item_id = itemId;
            this.count = count;
        }

        public ItemVO CreateItemVo()
        {
            return new ItemVO(item_id, count);
        }
    }

    public class ItemDB
    {
        //key : itemId

        public ItemDB(Dictionary<int, ItemData> db)
        {
            data = db;
        }
        public Dictionary<int, ItemData> data { get; private set; } = new Dictionary<int, ItemData>();
    }

    public class DBData
    {
        [JsonProperty("ID")]
        public int ID { get; protected set; }
    }
    public class ItemEffect : DBData
    {
        [JsonProperty("Runtime")]
        public float Runtime;
        [JsonProperty("AfterRuntime")]
        public float AfterRuntime;
        [JsonProperty("Effect_Type")]
        public EffectType Type;
        [JsonProperty("EffectValue")]
        public float Value;
        [JsonProperty("Effect_Range")]
        public int Radius;
        [JsonProperty("Effect_Range2")]
        public int Range;
        [JsonProperty("RunningTime")]
        public float Duration;
        [JsonProperty("CoolTime")]
        public float CoolTime;
        [JsonProperty("Durability")]
        public float Durability;
    }

    public class ItemEffectRelation : DBData
    {
        [JsonProperty("Item_ID")]
        public int ItemID;
    }

    //추후 리팩할게요...(Rename...ㅠ)
    public class ItemData : DBData
    {
        [JsonProperty("Item_Name")]
        public string name { get; private set; }
        [JsonProperty("Item_Description")]
        public string description{ get; private set; }
        [JsonProperty("Item_Type")]
        public ItemType itemType { get; private set; }
        [JsonProperty("Item_Weight")]
        public int weight { get; private set; }
        [JsonProperty("Item_Skill_ID")]
        public int skill_id { get; private set; }
        [JsonProperty("Item_Min_Dmg")]
        public int min_damage { get; private set; }
        [JsonProperty("Item_Max_Dmg")]
        public int max_damage { get; private set; }
        [JsonProperty("Item_OvelapCount")]
        public int overlapCount { get; private set; }
        public Sprite image;

        public ItemData(int id, string name, string description, ItemType itemType, int weight, int skill_id, int min_damage, int max_damage, int overlapCount, Sprite image)
        {
            ID = id;
            this.name = name;
            this.description = description;
            this.itemType = itemType;
            this.weight = weight;
            this.skill_id = skill_id;
            this.min_damage = min_damage;
            this.max_damage = max_damage;
            this.overlapCount = overlapCount;
            Sprite sprite = Managers.Resources.Load<Sprite>($"Arts/Items/{name}");
            if (sprite is not null) this.image = sprite;
        }
    }

    //인벤토리(아이템 컨테이너) 데이터
    public class ContainerVO
    {
        public ContainerVO(int maxCapacity)
        {
            this.maxCapacity = maxCapacity;
        }
        public ContainerVO(Dictionary<int, ItemVO> slots, int maxCapacity)
        {
            this.slots = slots;
            this.maxCapacity = maxCapacity;
        }
        public int maxCapacity { get; set; }
        //key: slot number
        public Dictionary<int, ItemVO> slots;

        public bool fullSlot
        {
            get
            {
                Debug.Log(slots.Keys.Count + ", " + maxCapacity);
                if (slots.Count == maxCapacity - 1) return true;
                return false;
            }
        }
    }
}
