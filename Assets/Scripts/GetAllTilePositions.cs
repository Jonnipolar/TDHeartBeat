using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetAllTilePositions : MonoBehaviour
{

    public Tilemap tileMap;
 
    public List<Vector3> availablePlaces;
    public List<Vector3Int> availablePlacesCell;
 
    void Awake () 
    {
        tileMap = transform.GetComponentInParent<Tilemap>();
        availablePlaces = new List<Vector3>();
 
        for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
        {
            for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
            {
                Vector3Int cellSpace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                Vector3 place = tileMap.CellToWorld(cellSpace);
                if (tileMap.HasTile(cellSpace))
                {
                    //Tile at "place"
                    availablePlaces.Add(place);
                    availablePlacesCell.Add(cellSpace);
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }

    void Start()
    {
        for(var i = 0; i < availablePlaces.Count; i++)
        {
            Debug.Log($"World: {availablePlaces[i]} \nCell: {availablePlacesCell[i]}");
        }

        Debug.Log($"Size of available places: {availablePlaces.Count}");
        Debug.Log($"Size of available places Cell: {availablePlacesCell.Count}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
