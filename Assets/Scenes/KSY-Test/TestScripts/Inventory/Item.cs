using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item
{
    public string name { get; private set; }
    public string comment { get; private set; }
    public int weight { get; private set; }
    public int overlapCount { get; private set; }
    public Sprite icon { get; private set; }

    public Action UseItem;

    public Item(string name, string comment = "", int weight = 0, int overlapCount = 10, Action UseItem = null)
    {
        this.name = name;
        this.comment = comment;
        this.weight = weight;
        this.overlapCount = overlapCount;
        this.UseItem = UseItem;

        icon = Managers.Resources.Load<Sprite>($"Arts/Items/{name}");
    }
}
