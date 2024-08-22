using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoliderButton : MonoBehaviour
{
    /// <summary>
    /// 버튼
    /// </summary>
    Button[] buttons;

    /// <summary>
    /// 업그레이드 클래스
    /// </summary>
    Upgrade upgrade;

    public Action onBarbarianButtonClick;
    public Action onWarriorButtonClick;
    public Action onWizardButtonClick;

    Player player;

    GameManager gameManager;


    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(Barbarian);
        buttons[1].onClick.AddListener(Warrior);
        buttons[2].onClick.AddListener(Wizard);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        upgrade = FindAnyObjectByType<Upgrade>();
        player = GameManager.Instance.Player;
    }

    /// <summary>
    /// 바바리안 버튼을 클릭했을 때 실행되는 함수
    /// </summary>
    private void Barbarian()
    {
        Debug.Log($"바바리안 바튼 클릭");
        if(gameManager.Money >= player.barbarianPrice)       // 게임매니저의 남은 돈이 바바리안의 가격보다 많으면
        {
            upgrade.SetActiveFalse();
            onBarbarianButtonClick?.Invoke();
            player.boardClickAble = true;
        }
    }

    /// <summary>
    /// 전사 버튼을 클릭했을 때 실행되는 함수
    /// </summary>
    private void Warrior()
    {
        Debug.Log($"전사 버튼 클릭");
        if (gameManager.Money >= player.warriorPrice)       // 게임매니저의 남은 돈이 전사의 가격보다 많으면
        {
            upgrade.SetActiveFalse();
            onWarriorButtonClick?.Invoke();
            player.boardClickAble = true;
        }
    }

    /// <summary>
    /// 마법사 버튼을 클릭했을 때 실행되는 함수
    /// </summary>
    private void Wizard()
    {
        Debug.Log($"마법사 버튼 클릭");
        if (gameManager.Money >= player.wizardPrice)       // 게임매니저의 남은 돈이 마법사의 가격보다 많으면
        {
            upgrade.SetActiveFalse();
            onWizardButtonClick?.Invoke();
            player.boardClickAble = true;       // soldier를 클릭한 상태에서 UI 가 나타났을 때 이 버튼을 누르면 설치가 안되는 문제 해결용
        }
    }
}
