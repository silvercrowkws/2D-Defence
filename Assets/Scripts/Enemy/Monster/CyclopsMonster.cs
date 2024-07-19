using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsMonster : MonsterBase
{
    protected override void Start()
    {
        moveSpeed = 2.5f;
        waitTime = 0.5f;
        hp = 100.0f;
        base.Start();
    }
}
