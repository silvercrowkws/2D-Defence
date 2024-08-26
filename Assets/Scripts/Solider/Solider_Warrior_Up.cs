using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider_Warrior_Up : AttackBase
{
    protected override void Start()
    {
        damage = 30.0f;
        attackSpeed = 0.75f;
        base.Start();
    }
}
