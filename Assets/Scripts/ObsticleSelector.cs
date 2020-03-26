using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ObsticleSelector : MonoBehaviour
{
    public Tilemap vainTilemap;
    public Tilemap bodyTilemap;
    public Sprite obsticle;

    Vector3 selectedTilePos;
    Vector3 maxVector;
    TileHighlight tileHighlight;

    void Start()
    {
        maxVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        selectedTilePos = maxVector;
        tileHighlight = GetComponent<TileHighlight>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Input listeners
         */

        // Register selected tile position on primary click
        // Add tower if same cell is clicked twice
        if (Input.GetMouseButtonDown(0)) 
        {
            tileHighlight.CallUpdate();

            //Get tile postition
            Vector3 cellPos = GetCellPosAtMouse();

            if (vainTilemap.HasTile(vainTilemap.WorldToCell(cellPos)))
            {
                AddObsticle(cellPos);
                selectedTilePos = maxVector;
            }
            // selectedTilePos = cellPos;
            // Add tower if same cell last clicked
            // if(selectedTilePos == cellPos) 
            // {
            //     selectedTilePos = maxVector;
            // }
            // else 
            // {
            //     // Register tile postition
            // }
            
        }
        // Unregister selected tile position on secondary click
        else if (Input.GetMouseButtonDown(1))
        {
            selectedTilePos = maxVector;
        }
    
        /*
         * Selection rendering
        */
        // Highlight on hover
        if(selectedTilePos == maxVector)
        {
            tileHighlight.CallUpdate();
        }

    }

    Vector3 GetCellPosAtMouse() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPos = ray.GetPoint(-ray.origin.z / ray.direction.z);
        return vainTilemap.GetCellCenterWorld(vainTilemap.WorldToCell(worldPos));
    }

    void AddObsticle(Vector3 pos)
    {
        Tile tileHighlight = ScriptableObject.CreateInstance<Tile>();
        tileHighlight.sprite = obsticle;
        bodyTilemap.SetTile(bodyTilemap.WorldToCell(pos), tileHighlight);
        vainTilemap.SetTile(vainTilemap.WorldToCell(pos), null);

        // set coroutine to remove it in x amount of time ** last **
        // remember to save the sprite before removing it in vain
        // reset vain at the end of coroutine


    }
}
