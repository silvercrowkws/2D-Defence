using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 인풋 시스템
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// castleTilemap을 연결할 변수
    /// </summary>
    public Tilemap castleTilemap;

    /// <summary>
    /// soliderTilemap을 연결할 변수
    /// </summary>
    public Tilemap soliderTilemap;

    /// <summary>
    /// groundTilemap을 연결할 변수
    /// </summary>
    public Tilemap groundTilemap;

    /// <summary>
    /// enemyTilemap을 연결할 변수
    /// </summary>
    public Tilemap enemyTilemap;

    /// <summary>
    /// barbarianTile 을 연결할 변수
    /// </summary>
    public TileBase barbarianTile;

    /// <summary>
    /// barbarianTile2 을 연결할 변수
    /// </summary>
    public TileBase barbarianTile2;

    /// <summary>
    /// warriorTile 을 연결할 변수
    /// </summary>
    public TileBase warriorTile;

    /// <summary>
    /// warriorTile2 을 연결할 변수
    /// </summary>
    public TileBase warriorTile2;

    /// <summary>
    /// wizardTile 을 연결할 변수
    /// </summary>
    public TileBase wizardTile;

    /// <summary>
    /// wizardTile2 을 연결할 변수
    /// </summary>
    public TileBase wizardTile2;

    /// <summary>
    /// 공격 범위 게임 오브젝트
    /// </summary>
    public GameObject collider_2_Tile;      // 바바리안 공격 범위
    public GameObject collider_3_Tile;      // 전사 공격 범위
    public GameObject collider_4_Tile;      // 마법사 공격 범위

    /// <summary>
    /// 강화된 공격 범위 게임 오브젝트
    /// </summary>
    public GameObject collider_2_Tile_Up;   // 강화 바바리안 공격 범위
    public GameObject collider_3_Tile_Up;   // 강화 워리어 공격 범위
    public GameObject collider_4_Tile_Up;   // 강화 마법사 공격 범위

    /// <summary>
    /// enemy 타일을 연결할 변수
    /// </summary>
    public TileBase enemyTile;      // 이건 타일맵이 아니라 특정 타일 하나인듯

    /// <summary>
    /// 선택된 타일
    /// </summary>
    TileBase clickedTile;

    /// <summary>
    /// 마우스를 따라다니는 클래스?
    /// </summary>
    FollowMouse followMouse;

    /// <summary>
    /// 솔저를 그 위치에 설치할 수 있는지 확인하는 변수
    /// </summary>
    public bool setAble = false;

    /// <summary>
    /// 솔져를 강화할 수 있는지 확인하는 변수
    /// </summary>
    public bool upgradeAble = false;

    /// <summary>
    /// 보드를 클릭할 수 있는지 확인하는 변수(solider를 누른게 아니면 true, solider를 눌렀으면 false)
    /// </summary>
    public bool boardClickAble = true;

    /// <summary>
    /// 선택한 타일의 트랜스폼
    /// </summary>
    //Transform clickedTileTransform;

    /// <summary>
    /// 선택한 솔져의 위치를 전달하는 델리게이트
    /// </summary>
    public Action<Vector3Int> onClickedTileTransform;

    /// <summary>
    /// 선택한 보드의 위치
    /// </summary>
    public Action<Vector3Int> onCellPosition;

    /// <summary>
    /// 각 solider를 클릭했을 때 공격 범위를 보이게 할 델리게이트
    /// </summary>
    //public Action onSoliderCilck;
    public Action<GameObject> onSoliderClick;

    /// <summary>
    /// solider 이외의 것을 클릭했을 때 공격 범위를 안보이게 할 델리게이트
    /// </summary>
    public Action onNonSoliderClick;

    /// <summary>
    /// 클릭한 셀의 위치
    /// </summary>
    Vector3Int cellPosition;

    /// <summary>
    /// Upgrade 클래스
    /// </summary>
    Upgrade upgrade;

    /// <summary>
    /// 설치된 솔저 오브젝트 Dictionary(키 : 위치, 값 : 오브젝트)
    /// </summary>
    public Dictionary<Vector3Int, GameObject> soliderObjectDictionary = new Dictionary<Vector3Int, GameObject>();

    /// <summary>
    /// 설치된 오브젝트 솔저 Dictionary(키 : 오브젝트, 값 : 위치)
    /// </summary>
    public Dictionary<GameObject, Vector3Int> objectSoliderDictionary = new Dictionary<GameObject, Vector3Int>();

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 바바리안의 가격
    /// </summary>
    public float barbarianPrice = 10.0f;

    /// <summary>
    /// 전사의 가격
    /// </summary>
    public float warriorPrice = 20.0f;

    /// <summary>
    /// 마법사의 가격
    /// </summary>
    public float wizardPrice = 50.0f;

    /// <summary>
    /// 강화 바바리안의 가격
    /// </summary>
    float barbarian2Price;

    /// <summary>
    /// 강화 전사의 가격
    /// </summary>
    float warrior2Price;

    /// <summary>
    /// 강화 마법사의 가격
    /// </summary>
    float wizard2Price;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        boardClickAble = true;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.PlayerClicks.Click.performed += BoardClick;
        inputActions.PlayerClicks.RClick.performed += NonPick;
    }

    private void OnDisable()
    {
        inputActions.PlayerClicks.Click.performed -= BoardClick;
        inputActions.PlayerClicks.RClick.performed -= NonPick;
        inputActions.Disable();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        followMouse = FindAnyObjectByType<FollowMouse>();
        upgrade = FindAnyObjectByType<Upgrade>();
        upgrade.onDestroyButton += SoliderDestroy;
        upgrade.onUpgradeYesButton += SoliderUpgrade;

        barbarian2Price = barbarianPrice + (barbarianPrice * 0.5f);     // 15원
        warrior2Price = warriorPrice + (warriorPrice * 0.5f);           // 30원
        wizard2Price = wizardPrice + (wizardPrice * 0.5f);              // 75원
    }

    /// <summary>
    /// solider 배치를 위해 버튼을 누른 상태에서 우클릭하면 취소되는 함수
    /// </summary>
    /// <param name="context"></param>
    private void NonPick(InputAction.CallbackContext context)
    {
        Debug.Log("우클릭 누름");
        followMouse.SetFollowImageColorDisable();         // 마우스를 따라다니던 이미지 비활성화
    }

    /// <summary>
    /// 클릭 함수
    /// </summary>
    /// <param name="context"></param>
    private void BoardClick(InputAction.CallbackContext context)
    {
        if(boardClickAble)      // solider를 누른게 아니면 true, solider를 눌렀으면 false
        {
            // 현재 마우스 위치를 스크린 좌표로 가져옴
            Vector2 screenPosition = Mouse.current.position.ReadValue();

            // 스크린 좌표를 월드 좌표로 변환
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            worldPosition.z = 0; // 2D 타일맵에서 z 좌표는 0으로 설정

            bool foundTile = false;

            // 타일맵 배열을 만들어서 각 타일맵을 순회하면서 검사
            Tilemap[] tilemaps = { soliderTilemap, enemyTilemap, castleTilemap, groundTilemap};     // 타일맵 순서 주의

            foreach (Tilemap tilemap in tilemaps)
            {
                if (IsTileAtPosition(tilemap, worldPosition))
                {
                    foundTile = true;
                    //Debug.Log($"{tilemap.name} 타일맵에서 타일을 찾음");
                    cellPosition = tilemap.WorldToCell(worldPosition);   // 셀 위치 가져오기

                    if (foundTile)
                    {
                        // 타일이 soliderTilemap인 경우
                        if (tilemap == soliderTilemap)
                        {
                            boardClickAble = false;
                            setAble = false;                            // 설치 가능 변수 off
                            Debug.Log("soliderTilemap 클릭");

                            followMouse.SetFollowImageColorDisable();         // 마우스를 따라다니던 이미지 비활성화

                            cellPosition = tilemap.WorldToCell(worldPosition);   // 셀 위치 가져오기
                            //Debug.Log($"cellPosition : {cellPosition}");
                            onClickedTileTransform?.Invoke(cellPosition);      // 델리게이트로 현재 solider가 클릭된 위치를 보냄

                            //Vector3 tileWorldPosition = tilemap.CellToWorld(cellPosition);  // 월드 좌표 가져오기
                            //Debug.Log($"tileWorldPosition : {tileWorldPosition}");

                            // 클릭된 solider 가져오기
                            if (soliderObjectDictionary.TryGetValue(cellPosition, out GameObject clickedSolider))
                            {
                                if (clickedSolider != null)
                                {
                                    Debug.Log($"누른 게임 오브젝트 : {clickedSolider}");

                                    onSoliderClick?.Invoke(clickedSolider); // 클릭된 solider 전달

                                    /*if (clickedTile == barbarianTile || clickedTile == warriorTile || clickedTile == wizardTile)
                                    {
                                        Debug.Log($"{clickedTile.name} 선택");
                                        upgradeAble = true; // 강화 가능 변수 on
                                    }*/
                                }
                            }

                            if (clickedTile == barbarianTile)
                            {
                                Debug.Log($"{clickedTile.name} 선택");
                                upgradeAble = true;                     // 강화 가능 변수 on

                                //Debug.Log(tilemap.transform);
                                // 이 위치를 델리게이트로 보내서 UI 관리하는 클래스에서 Ok, No 버튼의 위치를 조정하는 건데
                                // UI 버튼 같은거 OK, NO 띄우고 OK 누르면 강화, No 누르면 취소
                            }
                            else if(clickedTile == warriorTile)
                            {
                                Debug.Log($"{clickedTile.name} 선택");
                                upgradeAble = true;                     // 강화 가능 변수 on
                            }
                            else if(clickedTile == wizardTile)
                            {
                                Debug.Log($"{clickedTile.name} 선택");
                                upgradeAble = true;                     // 강화 가능 변수 on
                            }
                        }

                        // 타일이 enemyTilemap인 경우
                        else if (tilemap == enemyTilemap)
                        {
                            Debug.Log("enemyTilemap 클릭");
                            onNonSoliderClick?.Invoke();
                        }

                        // 타일이 castleTilemap인 경우
                        else if (tilemap == castleTilemap)
                        {
                            onNonSoliderClick?.Invoke();

                            Debug.Log("castleTilemap 클릭");
                            if (followMouse.soliderButtonOn)            // 솔져 버튼이 눌러진 상태이면
                            {
                                Debug.Log("해당 위치에 설치 가능");
                                //onCellPosition?.Invoke(cellPosition);      // 델리게이트로 현재 solider가 클릭된 위치를 보냄
                                SoliderSet(cellPosition);

                                setAble = true;                         // 설치 가능 변수 on
                                // 여기에 해당 위치에 설치를 묻는 작업 필요

                                // 다음으로 SoliderSet 함수 필요
                            }
                        }

                        // 타일이 groundTilemap인 경우
                        else if (tilemap == groundTilemap)
                        {
                            onNonSoliderClick?.Invoke();
                            Debug.Log("groundTilemap 클릭");
                        }

                        // 타일을 못찾았을 경우
                        else
                        {
                            onNonSoliderClick?.Invoke();
                            Debug.Log("못찾음");
                        }
                    }
                    break;      // 타일을 찾았으면 더 이상 검사하지 않음
                }
            }

            if (!foundTile)
            {
                Debug.Log("해당 위치에 타일이 없음");
                Debug.Log(foundTile);
            }

        }

    }

    private bool IsTileAtPosition(Tilemap tilemap, Vector3 worldPosition)
    {
        // 월드 좌표를 타일맵의 셀 좌표로 변환
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // 클릭한 위치에 타일이 있는지 확인
        clickedTile = tilemap.GetTile(cellPosition);

        return clickedTile != null;
    }

    /// <summary>
    /// 솔저를 배치하는 함수(게임 매니저에서 Money 차감)
    /// </summary>
    private void SoliderSet(Vector3Int soliderPosition)
    {
        Debug.Log("SoliderSet 함수 호출됨");
        Debug.Log($"현재 soliderImage: {followMouse.soliderImage}");
        Debug.Log($"soliderPosition: {soliderPosition}");

        //Vector3Int boardCell = new Vector3Int(soliderPosition.x, soliderPosition.y, soliderPosition.z);
        //Debug.Log(soliderPosition);

        Vector3 centerPosition = new Vector3(0.5f, 0.5f, 0);        // solider의 중앙 위치를 맞추기 위해

        GameObject createdObject = null;

        if (followMouse.soliderImage.sprite == followMouse.soliders[0])             // 바바리안을 클릭했었다
        {
            gameManager.Money -= barbarianPrice;                                    // 바바리안의 가격만큼 차감
            soliderTilemap.SetTile(soliderPosition, barbarianTile);
            createdObject = Instantiate(collider_2_Tile, soliderPosition + centerPosition, Quaternion.identity);        // 공격 범위 게임오브젝트 추가
            createdObject.name = "Barbarian";       // 이름 변경
            createdObject.transform.parent = this.gameObject.transform;     // 부모 설정
            Debug.Log("바바리안 타일 설치");
        }
        else if(followMouse.soliderImage.sprite == followMouse.soliders[1])         // 전사를 클릭했었다
        {
            gameManager.Money -= warriorPrice;                                    // 전사의 가격만큼 차감
            soliderTilemap.SetTile(soliderPosition, warriorTile);
            createdObject = Instantiate(collider_3_Tile, soliderPosition + centerPosition, Quaternion.identity);        // 공격 범위 게임오브젝트 추가
            createdObject.name = "Warrior";
            createdObject.transform.parent = this.gameObject.transform;
            Debug.Log("전사 타일 설치");
        }
        else if(followMouse.soliderImage.sprite == followMouse.soliders[2])         // 마법사를 클릭했었다
        {
            gameManager.Money -= wizardPrice;                                    // 마법사의 가격만큼 차감
            soliderTilemap.SetTile(soliderPosition, wizardTile);
            createdObject = Instantiate(collider_4_Tile, soliderPosition + centerPosition, Quaternion.identity);        // 공격 범위 게임오브젝트 추가
            createdObject.name = "Wizard";
            createdObject.transform.parent = this.gameObject.transform;
            Debug.Log("마법사 타일 설치");

            // soliderPosition 자리에 soliderTilemap.CellToWorld(soliderPosition)게 맞나?
        }

        if (createdObject != null)
        {
            soliderObjectDictionary[soliderPosition] = createdObject;   // Dictionary에 오브젝트 추가
            objectSoliderDictionary[createdObject] = soliderPosition;   // Dictionary에 오브젝트 추가
        }

        followMouse.SetFollowImageColorDisable();
    }

    /// <summary>
    /// 솔저를 강화하는 함수
    /// </summary>
    private void SoliderUpgrade()
    {
        Vector3 centerPosition = new Vector3(0.5f, 0.5f, 0);        // solider의 중앙 위치를 맞추기 위해

        GameObject createdObject = null;

        if (upgradeAble)            // 강화가 가능하면
        {
            Debug.Log("강화 함수 수행");

            //SoliderDestroy();       // 해당 위치에 있던 솔저 파괴

            if(clickedTile == barbarianTile && gameManager.Money >= barbarianPrice * 0.5f)     // 클릭 타일이 바바리안이고 강화 가능한 돈 이상 있으면
            {
                SoliderDestroy(false);       // 해당 위치에 있던 솔저 파괴

                gameManager.Money -= barbarianPrice * 0.5f;                    // 바바리안의 절반 가격만큼 차감
                soliderTilemap.SetTile(cellPosition, barbarianTile2);       // 바바리안 타일을 파괴하고 바바리안2 타일 설치                

                createdObject = Instantiate(collider_2_Tile_Up, cellPosition + centerPosition, Quaternion.identity);        // 공격 범위 게임오브젝트 추가
                createdObject.name = "Barbarian2";
            }
            else if(clickedTile == warriorTile && gameManager.Money >= warriorPrice * 0.5f)    // 클릭 타일이 전사이고 강화 가능한 돈 이상 있으면
            {
                SoliderDestroy(false);       // 해당 위치에 있던 솔저 파괴

                gameManager.Money -= warriorPrice * 0.5f;                      // 전사의 절반 가격만큼 차감
                soliderTilemap.SetTile(cellPosition, warriorTile2);         // 전사 타일을 파괴하고 워리어2 타일 설치                

                createdObject = Instantiate(collider_3_Tile_Up, cellPosition + centerPosition, Quaternion.identity);        // 공격 범위 게임오브젝트 추가
                createdObject.name = "Warrior2";
            }
            else if(clickedTile == wizardTile && gameManager.Money >= wizardPrice * 0.5f)      // 클릭 타일이 마법사이고 강화 가능한 돈 이상 있으면
            {
                SoliderDestroy(false);       // 해당 위치에 있던 솔저 파괴

                gameManager.Money -= wizardPrice * 0.5f;                       // 마법사의 절반 가격만큼 차감
                soliderTilemap.SetTile(cellPosition, wizardTile2);          // 위자드 타일을 파괴하고 위자드2 타일 설치                

                createdObject = Instantiate(collider_4_Tile_Up, cellPosition + centerPosition, Quaternion.identity);        // 공격 범위 게임오브젝트 추가
                createdObject.name = "Wizard2";
            }

            if (createdObject != null)
            {
                soliderObjectDictionary[cellPosition] = createdObject;   // Dictionary에 오브젝트 추가
                objectSoliderDictionary[createdObject] = cellPosition;   // Dictionary에 오브젝트 추가
            }

            followMouse.SetFollowImageColorDisable();
        }
    }

    /// <summary>
    /// 설치된 솔저를 파괴하는 함수
    /// </summary>
    private void SoliderDestroy(bool returnMoney)
    {
        if (returnMoney)
        {
            if(clickedTile == barbarianTile)
            {
                gameManager.Money += MathF.Floor(barbarianPrice * 0.5f);        // 바바리안을 파괴하면 바바리안 가격의 절반만큼 돌려줌
            }
            else if(clickedTile == warriorTile)
            {
                gameManager.Money += MathF.Floor(warriorPrice * 0.5f);          // 전사를 파괴하면 전사 가격의 절반만큼 돌려줌
            }
            else if(clickedTile == wizardTile)
            {
                gameManager.Money += MathF.Floor(wizardPrice * 0.5f);           // 마법사를 파괴하면 마법사 가격의 절반만큼 돌려줌
            }
            else if (clickedTile == barbarianTile2)
            {
                gameManager.Money += MathF.Floor(barbarian2Price * 0.5f);       // 강화 바바리안을 파괴하면 강화 바바리안 가격의 절반만큼 돌려줌
            }
            else if (clickedTile == warriorTile2)
            {
                gameManager.Money += MathF.Floor(warrior2Price * 0.5f);         // 강화 전사를 파괴하면 강화 전사 가격의 절반만큼 돌려줌
            }
            else if (clickedTile == wizardTile2)
            {
                gameManager.Money += MathF.Floor(wizard2Price * 0.5f);          // 강화 마법사를 파괴하면 강화 마법사 가격의 절반만큼 돌려줌
            }
        }

        soliderTilemap.SetTile(cellPosition, null);

        // Dictionary에서 해당 위치의 오브젝트 찾기
        if (soliderObjectDictionary.TryGetValue(cellPosition, out GameObject soliderToDestroy))     // cellPosition에 있는 게임 오브젝트를 soliderToDestroy라고 정하기
        {
            //Destroy(soliderToDestroy); // 오브젝트 파괴

            if (soliderToDestroy != null)
            {
                Destroy(soliderToDestroy); // 오브젝트 파괴
            }

            soliderObjectDictionary.Remove(cellPosition);       // Dictionary에서 제거
            objectSoliderDictionary.Remove(soliderToDestroy);   // Dictionary에서 제거
        }
    }


    /*private void CheckTileAtPosition(Tilemap tilemap, Vector3 worldPosition)
    {
        // 월드 좌표를 타일맵의 셀 좌표로 변환
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // 클릭한 위치에 타일이 있는지 확인
        clickedTile = tilemap.GetTile(cellPosition);

        if(tilemap == castleTilemap)
        {
            if (followMouse.soliderButtonOn && clickedTile != barbarianTile)     // 추가로 그 위에 솔저가 없는지 확인 필요
            {
                Debug.Log("해당 위치에 설치 가능");
            }
            else
            {
                Debug.Log("해당 위치에 설치 불가능");
            }
        }

        if (clickedTile != null)
        {
            Debug.Log($"Clicked on {tilemap.name} at {cellPosition}");

            if (clickedTile == barbarianTile)
            {
                // 클릭한 타일이 barbarianTile 타일인 경우
                Debug.Log($"{tilemap.name}에 {barbarianTile} 이 있다");
            }
            else if (clickedTile == enemyTile)
            {
                // 클릭한 타일이 enemyTile 타일인 경우
                Debug.Log($"{tilemap.name}에 {enemyTile} 이 있다.");
            }
            else
            {
                // 클릭한 타일이 지정된 타일이 아닌 경우
                Debug.Log("해당 위치에 지정된 타일 없음");
                // 지금은 모든 타일맵을 체크하고 있기 때문에 한 타일 위치에 여러 타일이 있을 경우 이것도 발생함
            }
        }
    }*/
}
