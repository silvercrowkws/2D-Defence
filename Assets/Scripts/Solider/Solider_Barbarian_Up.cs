using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider_Barbarian_Up : AttackBase
{
    protected override void Start()
    {
        damage = 15.0f;
        attackSpeed = 0.5f;
        base.Start();
    }
}
