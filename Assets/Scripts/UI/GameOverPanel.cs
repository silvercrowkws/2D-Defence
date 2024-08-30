using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    TurnManager turnManager;

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
    

    private void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        turnManager.onTurnOver += OnPanelChange;

        Transform child = transform.GetChild(0);

        starImage_0 = child.GetChild(0).GetComponent<Image>();
        starImage_1 = child.GetChild(1).GetComponent<Image>();
        starImage_2 = child.GetChild(2).GetComponent<Image>();

        this.gameObject.SetActive(false);       // 처음에 안보이게 하기
    }

    /// <summary>
    /// 턴 매니저의 onTurnOver 델리게이트를 받아 게임 종료 시 패널을 변경하는 함수
    /// </summary>
    /// <param name="doorMonster">문에 들어온 몬스터</param>
    private void OnPanelChange(int doorMonster)
    {
        this.gameObject.SetActive(true);

        if(doorMonster == 0)                            // 한마리도 들어오지 않았다
        {
            starImage_0.sprite = star[1];
            starImage_1.sprite = star[1];
            starImage_2.sprite = star[1];
        }
        else if(doorMonster > 0 && doorMonster < 6)     // 들어온 몬스터가 1 ~ 5마리 이다
        {
            starImage_0.sprite = star[1];
            starImage_1.sprite = star[1];
            starImage_2.sprite = star[0];
        }
        else if(doorMonster > 5 && doorMonster < 10)   // 들어온 몬스터가 6 ~ 9마리 이다
        {
            starImage_0.sprite = star[1];
            starImage_1.sprite = star[0];
            starImage_2.sprite = star[0];
        }
        else                                          // 10마리가 들어왔다 => 게임 실패
        {
            starImage_0.sprite = star[0];
            starImage_1.sprite = star[0];
            starImage_2.sprite = star[0];
        }
    }
}
