using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider_Wizard_Up : AttackBase
{
    protected override void Start()
    {
        damage = 75.0f;
        attackSpeed = 2.0f;
        base.Start();
    }
}
