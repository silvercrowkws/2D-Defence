using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

#if UNITY_EDITOR
    

#endif
}
