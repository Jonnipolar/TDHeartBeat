using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeTileTest : MonoBehaviour
{
    public Tilemap tilemap;
    public Sprite[] tile_change;
    public Vector3Int pos;
    public float startDelay = 0.0f;
    public float switchTime = 1.0f;
    private bool control = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeTileRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeTileRoutine()
    {
        yield return new WaitForSeconds(startDelay);
        while(true)
        {
            changeTileByCoord();
            yield return new WaitForSeconds(switchTime);
        }
    }

    public void changeTileByCoord()
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        if(control) { tile.sprite = tile_change[0]; }
        else { tile.sprite = tile_change[1]; }
        tilemap.SetTile(pos, tile);
        control = !control;
    }
}
