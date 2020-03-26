using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TDHeartBeat.Assets.Scripts.AStar;

public class GetAllTilePositions : MonoBehaviour
{
    public Tilemap tileMap;
    [HideInInspector]
    public List<Vector3> availablePlaces;
    [HideInInspector]
    public List<Vector3Int> availablePlacesCell;
    public Transform goalPosition;
    /*public Transform startPosition;*/
    
    // Private variables
    private AStarAgent agent;
    [HideInInspector]
    private List<Vector2Int> availableCells2D;
 
    // Start -8, 8
    // end 0 -2
    void Awake () 
    {
        // tileMap = transform.GetComponentInParent<Tilemap>();
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
            }
        }

        availableCells2D = new List<Vector2Int>();
        foreach (var pos in availablePlacesCell)
        {
            availableCells2D.Add(new Vector2Int(pos.x, pos.y));
        }

        var goal = tileMap.WorldToCell(goalPosition.transform.position);
        agent = new AStarAgent(new Vector2Int(goal.x, goal.y));
    }

    public Stack<Vector2> getMoves(List<Vector2Int> availableCells, Vector2Int positionNow)
    {
        return agent.getMoves(availableCells, positionNow, tileMap);
    }

    public Stack<Vector2> getMoves(List<Vector2Int> availableCells, Vector3 positionNow)
    {
        Vector3Int pos = tileMap.WorldToCell(positionNow);
        return agent.getMoves(availableCells, new Vector2Int(pos.x, pos.y), tileMap);
    }

    public List<Vector2Int> getAvailableCells()
    {
        return availableCells2D;
    }
}
