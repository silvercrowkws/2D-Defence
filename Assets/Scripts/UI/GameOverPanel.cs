using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    TurnManager turnManager;

    GameManager gameManager;

    /// <summary>
    /// star 스프라이트의 배열(0번이 빈 것, 1번이 차 있는 것)
    /// </summary>
    public Sprite[] star;

    /// <summary>
    /// Star 게임 오브젝트의 자식으로 있는 이미지들
    /// </summary>
    Image starImage_0;
    Image starImage_1;
    Image starImage_2;

    /// <summary>
    /// 재시작 버튼
    /// </summary>
    Button restartButton;
    

    private void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        turnManager.onTurnOver += OnPanelChange;

        gameManager = GameManager.Instance;

        Transform child = transform.GetChild(0);

        starImage_0 = child.GetChild(0).GetComponent<Image>();
        starImage_1 = child.GetChild(1).GetComponent<Image>();
        starImage_2 = child.GetChild(2).GetComponent<Image>();

        child = transform.GetChild(1);
        restartButton = child.GetComponent<Button>();
        restartButton.onClick.AddListener(RestartButton);

        //InitializeUIElements();

        this.gameObject.SetActive(false);       // 처음에 안보이게 하기
    }

    /*private void InitializeUIElements()
    {
        // 자식 오브젝트를 찾아서 Image 컴포넌트를 가져오기
        Transform child = transform.GetChild(0);
        starImage_0 = child.GetChild(0).GetComponent<Image>();
        starImage_1 = child.GetChild(1).GetComponent<Image>();
        starImage_2 = child.GetChild(2).GetComponent<Image>();

        child = transform.GetChild(1);
        restartButton = child.GetComponent<Button>();
        restartButton.onClick.AddListener(RestartButton);
    }*/

    /// <summary>
    /// 턴 매니저의 onTurnOver 델리게이트를 받아 게임 종료 시 패널을 변경하는 함수
    /// </summary>
    /// <param name="panelDoorLife">문의 체력</param>
    private void OnPanelChange(int panelDoorLife)
    {
        // GameOverPanel이 이미 파괴된 경우를 방지하기 위한 안전성 검사
        if (this == null || gameObject == null) return;


        this.gameObject.SetActive(true);

        if(panelDoorLife == 0)                                  // 문의 체력이 0이다(10마리가 들어왔다)
        {
            starImage_0.sprite = star[0];
            starImage_1.sprite = star[0];
            starImage_2.sprite = star[0];
        }        
        else if(panelDoorLife > 0 && panelDoorLife < 6)         // 문의 체력이 1 ~ 5이다
        {
            starImage_0.sprite = star[1];
            starImage_1.sprite = star[0];
            starImage_2.sprite = star[0];
        }
        else if (panelDoorLife > 5 && panelDoorLife < 10)       // 문의 체력이 6 ~ 9이다
        {
            starImage_0.sprite = star[1];
            starImage_1.sprite = star[1];
            starImage_2.sprite = star[0];
        }
        else if (panelDoorLife > 9)                             // 문의 체력이 10이다
        {
            starImage_0.sprite = star[1];
            starImage_1.sprite = star[1];
            starImage_2.sprite = star[1];
        }
        else                                                    // 오류?
        {
            Debug.Log("문의 체력 오류?");
        }
    }

    /// <summary>
    /// 재시작 버튼
    /// </summary>
    private void RestartButton()
    {
        Debug.Log("재시작 버튼 클릭");
        //this.gameObject.SetActive(false);
        gameManager.GameRestart();
        SceneManager.LoadScene(0);
    }
}
