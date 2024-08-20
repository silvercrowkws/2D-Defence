using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_10_Money : TestBase
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        gameManager.Money += 1;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        gameManager.Money += 10;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        gameManager.Money += 100;
    }
}
