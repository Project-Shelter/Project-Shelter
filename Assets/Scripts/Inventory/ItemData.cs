using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    UseItem,
    EctItem,
    EquipItem,
    ActionItem,
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
            }
        }
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

    public class ItemDB
    {
        //key : itemId
        public Dictionary<int, ItemData> data { get; private set; } = new Dictionary<int, ItemData>();
    }

    //DB에 정적으로 저장/아이템 id가 동일하면 내용이 같은 것
    //Entity? (Item_Skill_ID의 용도에 따라 달라질 듯 - 이게 프로그래밍 시 어떻게 될 지...)
    public class ItemData
    {
        public int id { get; private set; }
        public string name { get; private set; }
        public string description{ get; private set; }
        public ItemType itemType { get; private set; }
        public int weight { get; private set; }
        public int overlapCount { get; private set; }
        public Sprite image;

        public ItemData(int id, string name, string description, ItemType itemType, int weight, int overlapCount, Sprite image)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.itemType = itemType;
            this.weight = weight;
            this.overlapCount = overlapCount;
            this.image = image;
        }
    }

    //인벤토리(아이템 컨테이너) 데이터
    public class ContainerVO
    {
        public ContainerVO(int maxCapacity)
        {
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
