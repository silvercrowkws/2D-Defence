using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour
{
    private RectTransform rectTransform;

    /// <summary>
    /// 솔저 버튼
    /// </summary>
    SoliderButton soliderButton;

    /// <summary>
    /// 자식의 이미지
    /// </summary>
    public Image soliderImage;

    /// <summary>
    /// 이미지의 컬러
    /// </summary>
    Color imageColor;

    /// <summary>
    /// 버튼의 이미지로 사용할 스프라이트들
    /// </summary>
    public Sprite[] soliders;

    /// <summary>
    /// solider 버튼이 눌러졌는지 확인하는 변수(캐릭터 픽하는 버튼)
    /// </summary>
    public bool soliderButtonOn = false;

    /// <summary>
    /// 공격 범위 UI들
    /// </summary>
    GameObject barbarianCircle;
    GameObject warriorCircle;
    GameObject wizardCircle;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        soliderImage = GetComponentInChildren<Image>();
        soliderImage.sprite = null;
        
        if (soliderImage != null)      // 이미지를 찾았으면 알파값을 0으로 조정
        {
            imageColor = soliderImage.color;
            imageColor.a = 0f;
            soliderImage.color = imageColor;
        }

        Transform child = transform.GetChild(1);
        barbarianCircle = child.gameObject;
        barbarianCircle.gameObject.SetActive(false);

        child = transform.GetChild(2);
        warriorCircle = child.gameObject;
        warriorCircle.gameObject.SetActive(false);

        child = transform.GetChild(3);
        wizardCircle = child.gameObject;
        wizardCircle.gameObject.SetActive(false);
    }

    void Start()
    {
        soliderButton = FindAnyObjectByType<SoliderButton>();
        soliderButton.onBarbarianButtonClick += OnBarbarianButtonClick;
        soliderButton.onWarriorButtonClick += OnWarriorButtonClick;
        soliderButton.onWizardButtonClick += OnWizardButtonClick;
    }

    void Update()
    {
        // 마우스 위치를 스크린 좌표로 가져옵니다.
        Vector2 mousePosition = Input.mousePosition;

        // RectTransform의 부모 Canvas를 기준으로 마우스 위치를 계산합니다.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, mousePosition, null, out Vector2 localPoint);

        // 계산된 로컬 좌표를 RectTransform의 위치로 설정합니다.
        rectTransform.localPosition = localPoint;
    }

    /// <summary>
    /// 바바리안 버튼이 클릭되었을 때 실행되는 함수
    /// </summary>
    private void OnBarbarianButtonClick()
    {
        SetFollowImageColor();
        soliderImage.sprite = soliders[0];
        barbarianCircle.gameObject.SetActive(true);
    }

    /// <summary>
    /// 전사 버튼이 클릭되었을 때 실행되는 함수
    /// </summary>
    private void OnWarriorButtonClick()
    {
        SetFollowImageColor();
        soliderImage.sprite = soliders[1];
        warriorCircle.gameObject.SetActive(true);
    }

    /// <summary>
    /// 마법사 버튼이 클릭되었을 때 실행되는 함수
    /// </summary>
    private void OnWizardButtonClick()
    {
        SetFollowImageColor();
        soliderImage.sprite = soliders[2];
        wizardCircle.gameObject.SetActive(true);
    }

    /// <summary>
    /// 버튼이 클릭되었을 때 마우스에 따라다니는 이미지의 알파를 조절하는 함수
    /// solider를 설치할 수 있는지 확인하는 bool변수 컨트롤 중
    /// </summary>
    void SetFollowImageColor()
    {
        imageColor.a = 1.0f;
        soliderImage.color = imageColor;
        soliderButtonOn = true;
    }

    /// <summary>
    /// 버튼 클릭이 취소되었을 때 실행되는 함수
    /// </summary>
    public void SetFollowImageColorDisable()
    {
        imageColor.a = 0.0f;
        soliderImage.color = imageColor;
        soliderButtonOn = false;

        if(barbarianCircle.gameObject.activeSelf)
        {
            barbarianCircle.gameObject.SetActive(false);
        }

        if(warriorCircle.gameObject.activeSelf)
        {
            warriorCircle.gameObject.SetActive(false);
        }

        if (wizardCircle.gameObject.activeSelf)
        {
            wizardCircle.gameObject.SetActive(false);
        }
    }
}
