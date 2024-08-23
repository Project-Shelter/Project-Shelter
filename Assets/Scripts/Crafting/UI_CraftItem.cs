using System;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

    public class UI_CraftItem : UI_Section
    {
        public int ID { get; private set; }
        private TextMeshProUGUI name;
        private TextMeshProUGUI comment;
        private Image icon;
        private Button button;

        enum Texts
        {
            ItemName,
            ItemComment,
        }
        enum Images
        {
            ItemIcon,
        }

        public override void Init()
        {
            base.Init();
            Bind<TextMeshProUGUI>(typeof(Texts));
            Bind<Image>(typeof(Images));

            name = GetText((int)Texts.ItemName);
            comment = GetText((int)Texts.ItemComment);
            icon = GetImage((int)Images.ItemIcon);
        }

        public void UpdateView(CraftItem item)
        {
            ID = item.ID;
            name.text = item.Name;
            comment.text = item.Comment;
            icon.sprite = item.Icon;
        }

        public void BindEvent(UnityAction action)
        {
            button.onClick.AddListener(action);
        }
        
    }
