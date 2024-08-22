using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameControllButton : MonoBehaviour
{
    /// <summary>
    /// X 1, X 2, X 3 텍스트
    /// </summary>
    TextMeshProUGUI xText;

    /// <summary>
    /// 게임 속도를 조정하는 버튼
    /// </summary>
    Button speedChangeButton;

    /// <summary>
    /// 게임을 일시정지하는 버튼
    /// </summary>
    Button gamePauseButton;

    /// <summary>
    /// pause 스프라이트와, run 스프라이트
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    /// 게임퓨즈버튼 이미지
    /// </summary>
    Image gamePauseButtonImage;

    int speedIndex = 0;
    int pauseIndex = 0;

    /// <summary>
    /// 게임 속도를 저장하기 위한 변수
    /// </summary>
    float gameSpeed = 1.0f;

    private void Awake()
    {
        Transform child = transform.GetChild(0);

        speedChangeButton = child.GetComponent<Button>();
        speedChangeButton.onClick.AddListener(GameSpeedChange);
        xText = child.GetComponentInChildren<TextMeshProUGUI>();


        child = transform.GetChild(1);
        gamePauseButton = child.GetComponent <Button>();
        gamePauseButton.onClick.AddListener(GamePause);
        gamePauseButtonImage = child.GetChild(0).GetComponent<Image>();

        Debug.Log("GamePauseButton Image Name: " + gamePauseButtonImage.name);
    }

    /// <summary>
    /// 게임 속도를 조정하는 함수
    /// </summary>
    private void GameSpeedChange()
    {
        if(Time.timeScale != 0)
        {
            // 처음 누르면 2배속, 다시 누르면 3배속, 또 다시 누르면 1배속 반복

            speedIndex++;        // 버튼을 누르면 speedIndex++

            // speedIndex = 1일 경우
            // 1 % 3 + 1 = 2

            // speedIndex = 2일 경우
            // 2 % 3 + 1 = 3

            // speedIndex = 3일 경우
            // 3 % 3 + 1 = 1

            switch (speedIndex%3 +1)
            {
                // 1배속
                case 1:
                    Time.timeScale = 1.0f;
                    xText.text = $"X 1";
                    gameSpeed = 1.0f;
                    break;
                // 2배속
                case 2:
                    Time.timeScale = 2.0f;
                    xText.text = $"X 2";
                    gameSpeed = 2.0f;
                    break;
                // 3배속
                case 3:
                    Time.timeScale = 3.0f;
                    xText.text = $"X 3";
                    gameSpeed = 3.0f;
                    break;
            }
        }
    }

    /// <summary>
    /// 게임을 정지, 해제시키는 함수
    /// </summary>
    private void GamePause()
    {
        // 처음 누르면 정지, 다시 누르면 해제
        pauseIndex++;
        switch(pauseIndex%2)
        {
            // 해제
            case 0:
                gamePauseButtonImage.sprite = sprites[0];
                Time.timeScale = gameSpeed;     // 변경된 속도로 해제
                break;
            // 정지
            case 1:
                gamePauseButtonImage.sprite = sprites[1];
                Time.timeScale = 0.0f;
                break;
        }
    }
}
