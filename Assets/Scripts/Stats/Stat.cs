using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 추후 Stat을 MonoBehaviour로 조정하지 않는 경우엔, 생성자로 baseValue 초기화
[System.Serializable]
public class Stat
{
    // 임시로 public. 추후에 생성자로 초기화할때는 private.
    public float baseValue;
    private List<float> modifiers = new List<float>();

    public float GetValue()
    {
        float finalValue = baseValue;
        modifiers.ForEach(modifier => finalValue += modifier);
        return finalValue;
    }

    public void AddModifier(float modifier)
    {
        if (modifier != 0)
            { modifiers.Add(modifier); }
    }

    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
            { modifiers.Remove(modifier); }

    }
}
