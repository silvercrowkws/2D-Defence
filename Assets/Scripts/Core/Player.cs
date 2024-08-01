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
    /// warriorTile 을 연결할 변수
    /// </summary>
    public TileBase warriorTile;

    /// <summary>
    /// wizardTile 을 연결할 변수
    /// </summary>
    public TileBase wizardTile;

    /// <summary>
    /// 공격 범위 게임 오브젝트
    /// </summary>
    public GameObject collider_2_Tile;      // 바바리안 공격 범위
    public GameObject collider_3_Tile;      // 전사 공격 범위
    public GameObject collider_4_Tile;      // 마법사 공격 범위

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
    /// 클릭한 셀의 위치
    /// </summary>
    Vector3Int cellPosition;

    /// <summary>
    /// Upgrade 클래스
    /// </summary>
    Upgrade upgrade;

    // 설치된 솔저 오브젝트 Dictionary
    private Dictionary<Vector3Int, GameObject> soliderObjects = new Dictionary<Vector3Int, GameObject>();


    private void Awake()
    {
        inputActions = new PlayerInputActions();
        boardClickAble = true;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.PlayerClicks.Click.performed += BoardClick;
    }

    private void OnDisable()
    {
        inputActions.PlayerClicks.Click.performed -= BoardClick;
        inputActions.Disable();
    }

    private void Start()
    {
        followMouse = FindAnyObjectByType<FollowMouse>();
        upgrade = FindAnyObjectByType<Upgrade>();
        upgrade.onDestroyButton += SoliderDestroy;
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

                            Vector3 tileWorldPosition = tilemap.CellToWorld(cellPosition);  // 월드 좌표 가져오기
                            //Debug.Log($"tileWorldPosition : {tileWorldPosition}");

                            if (clickedTile == barbarianTile)
                            {
                                Debug.Log($"{clickedTile.name} 선택");
                                upgradeAble = true;                     // 강화 가능 변수 on

                                Debug.Log(tilemap.transform);
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
                        }

                        // 타일이 castleTilemap인 경우
                        else if (tilemap == castleTilemap)
                        {
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
                            Debug.Log("groundTilemap 클릭");
                        }

                        // 타일을 못찾았을 경우
                        else
                        {
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
    /// 솔저를 배치하는 함수
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
            soliderTilemap.SetTile(soliderPosition, barbarianTile);
            createdObject = Instantiate(collider_2_Tile, soliderPosition + centerPosition, Quaternion.identity);        // 공격 범위 게임오브젝트 추가
            createdObject.name = "Barbarian";       // 이름 변경
            createdObject.transform.parent = this.gameObject.transform;     // 부모 설정
            Debug.Log("바바리안 타일 설치");
        }
        else if(followMouse.soliderImage.sprite == followMouse.soliders[1])        // 전사를 클릭했었다
        {
            soliderTilemap.SetTile(soliderPosition, warriorTile);
            createdObject = Instantiate(collider_3_Tile, soliderPosition + centerPosition, Quaternion.identity);        // 공격 범위 게임오브젝트 추가
            createdObject.name = "Warrior";
            createdObject.transform.parent = this.gameObject.transform;
            Debug.Log("전사 타일 설치");
        }
        else if(followMouse.soliderImage.sprite == followMouse.soliders[2])        // 마법사를 클릭했었다
        {
            soliderTilemap.SetTile(soliderPosition, wizardTile);
            createdObject = Instantiate(collider_4_Tile, soliderPosition + centerPosition, Quaternion.identity);        // 공격 범위 게임오브젝트 추가
            createdObject.name = "Wizard";
            createdObject.transform.parent = this.gameObject.transform;
            Debug.Log("마법사 타일 설치");

            // soliderPosition 자리에 soliderTilemap.CellToWorld(soliderPosition)게 맞나?
        }

        if (createdObject != null)
        {
            soliderObjects[soliderPosition] = createdObject;  // Dictionary에 오브젝트 추가
        }

        followMouse.SetFollowImageColorDisable();
    }

    /// <summary>
    /// 솔저를 강화하는 함수
    /// </summary>
    private void SoliderUpgrade()
    {

    }

    /// <summary>
    /// 설치된 솔저를 파괴하는 함수
    /// </summary>
    private void SoliderDestroy()
    {
        soliderTilemap.SetTile(cellPosition, null);

        // Dictionary에서 해당 위치의 오브젝트 찾기
        if (soliderObjects.TryGetValue(cellPosition, out GameObject soliderToDestroy))
        {
            Destroy(soliderToDestroy); // 오브젝트 파괴
            soliderObjects.Remove(cellPosition); // Dictionary에서 제거
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
