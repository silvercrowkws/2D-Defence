using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider_Warrior : AttackBase
{
    protected override void Start()
    {
        damage = 20.0f;     // 강화해서 50% 증가하면 30되게
        attackSpeed = 1.5f;
        base.Start();
    }
}
