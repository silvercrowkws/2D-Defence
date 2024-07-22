using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMonster_2 : MonsterBase
{
    protected override void Start()
    {
        moveSpeed = 3;
        waitTime = 0;
        hp = 75.0f;
        base.Start();
    }
}
