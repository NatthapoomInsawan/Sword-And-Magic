using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum TargetType
    {
        enemy,
        ally,
    }
    readonly private int minDamage;
    readonly private int maxDamage;
    public TargetType Target { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }

    public Item (string name, int min,int max, TargetType target, int amount)
    {
        Name = name;
        minDamage = min;
        maxDamage = max;
        Target = target;
        Amount = amount;
    }

    public int GetDamage()
    {
        return Random.Range(minDamage,maxDamage);
    }

}
