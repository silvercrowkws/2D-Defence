using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider_Wizard : AttackBase
{
    protected override void Start()
    {
        damage = 50.0f;     // 강화해서 50% 증가하면 75되게
        attackSpeed = 3.0f;
        base.Start();
    }
}
