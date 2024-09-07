using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    Button startButton;

    private void Awake()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(StartScene);
    }

    /// <summary>
    /// 씬을 시작시키는 버튼으로 실행되는 함수
    /// </summary>
    private void StartScene()
    {
        Debug.Log("startButton 클릭");
        SceneManager.LoadScene(1);
    }
}
