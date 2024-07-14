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

    public Action onBarbarianButtonClick;
    public Action onWarriorButtonClick;
    public Action onWizardButtonClick;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(Barbarian);
        buttons[1].onClick.AddListener(Warrior);
        buttons[2].onClick.AddListener(Wizard);
    }

    /// <summary>
    /// 바바리안 버튼을 클릭했을 때 실행되는 함수
    /// </summary>
    private void Barbarian()
    {
        Debug.Log($"바바리안 바튼 클릭");
        onBarbarianButtonClick?.Invoke();
    }

    /// <summary>
    /// 전사 버튼을 클릭했을 때 실행되는 함수
    /// </summary>
    private void Warrior()
    {
        Debug.Log($"전사 버튼 클릭");
        onWarriorButtonClick?.Invoke();
    }

    /// <summary>
    /// 마법사 버튼을 클릭했을 때 실행되는 함수
    /// </summary>
    private void Wizard()
    {
        Debug.Log($"마법사 버튼 클릭");
        onWizardButtonClick?.Invoke();
    }
}
