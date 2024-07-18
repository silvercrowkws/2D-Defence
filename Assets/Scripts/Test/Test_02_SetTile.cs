using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_02_SetTile : TestBase
{
#if UNITY_EDITOR
    public Tilemap soliderTilemap;

    public TileBase barbarianTile;

    Vector3Int testPosition = new Vector3Int(0, 0, 0);

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        soliderTilemap.SetTile(testPosition, barbarianTile);
    }
#endif
}
