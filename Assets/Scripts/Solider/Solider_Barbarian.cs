using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider_Barbarian : AttackBase
{
    protected override void Start()
    {
        damage = 10.0f;     // 강화해서 50% 증가하면 15되게
        attackSpeed = 1.0f;
        base.Start();
    }
}
