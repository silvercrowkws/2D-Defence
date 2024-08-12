using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_09_AttackRangeUI : TestBase
{
#if UNITY_EDITOR
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Time.timeScale = 20.0f;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Time.timeScale = 1.0f;
    }
#endif
}
