using System;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

    public class UI_CraftItem : UI_Section
    {
        public int ID { get; private set; }
        private TextMeshProUGUI itemName;
        private TextMeshProUGUI comment;
        private Image icon;
        private Button button;
        public Action<int> ClickCraftItem = null;

        enum Texts
        {
            ItemName,
            ItemComment,
        }
        enum Images
        {
            ItemIcon,
        }
        
        void Awake()
        {
            Init();
        }

        public override void Init()
        {
            base.Init();
            Bind<TextMeshProUGUI>(typeof(Texts));
            Bind<Image>(typeof(Images));

            itemName = GetText((int)Texts.ItemName);
            comment = GetText((int)Texts.ItemComment);
            icon = GetImage((int)Images.ItemIcon);
            
            button = gameObject.GetComponent<Button>();
            
            button?.onClick.AddListener(delegate
            {
                ClickCraftItem.Invoke(ID);
            });
        }

        public void UpdateView(CraftItem item)
        {
            ID = item.ID;
            itemName.text = item.Name;
            comment.text = item.Comment;
            icon.sprite = item.Icon;
        }

        public void BindEvent(Action<int> action)
        {
            ClickCraftItem -= action;
            ClickCraftItem += action;
        }
        
    }
