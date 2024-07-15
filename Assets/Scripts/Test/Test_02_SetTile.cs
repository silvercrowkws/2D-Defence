using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test_02_SetTile : MonoBehaviour
{
#if UNITY_EDITOR
    public Tilemap soliderTilemap;

    public TileBase barbarianTile;

    Vector3Int testPosition = new Vector3Int(0, 0, 0);

    private void Start()
    {
        soliderTilemap.SetTile(testPosition, barbarianTile);
    }
#endif
}
