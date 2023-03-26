using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public enum TargetType
    {
        self,
        enemy,
        ally,
    }
    readonly private int minDamage;
    readonly private int maxDamage;
    public TargetType Target { get; set; }
    public int Cost { get; set; }
    public string Name { get; set; }
    public bool OncePerFight{ get; set; }
    public bool IsUsed { get; set; }

    public Skill (string name, int min,int max, TargetType target, int cost, bool oncePerFight)
    {
        Name = name;
        minDamage = min;
        maxDamage = max;
        Target = target;
        Cost = cost;
        OncePerFight = oncePerFight;
    }

    public int GetDamage()
    {
        return Random.Range(minDamage,maxDamage);
    }

}
