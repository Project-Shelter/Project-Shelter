using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public class ItemData
    {
        public int id { get; private set; }
        public int weight { get; private set; }
        public int overlapCount { get; private set; }
        public string name { get; private set; }
        public string comment { get; private set; }
        public Sprite image;

        public ItemData(int id, int weight, int overlapCount, string name, string comment, Sprite image)
        {
            this.id = id;
            this.weight = weight;
            this.name = name;
            this.comment = comment;
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
    }
}
