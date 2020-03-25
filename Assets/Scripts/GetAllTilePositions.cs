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
    public Transform startPosition;
    
    // Private variables
    private AStarAgent agent;
    [HideInInspector]
    public Stack<Vector2> moves;
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
                else
                {
                    //No tile at "place"
                }
            }
        }

        availableCells2D = new List<Vector2Int>();
        foreach (var pos in availablePlacesCell)
        {
            availableCells2D.Add(new Vector2Int(pos.x, pos.y));
        }
    }

    void Start()
    {
        // for(var i = 0; i < availablePlaces.Count; i++)
        // {
        //     Debug.Log($"World: {availablePlaces[i]} \nCell: {availablePlacesCell[i]}");
        // }

        // Debug.Log($"Size of available places: {availablePlaces.Count}");
        // Debug.Log($"Size of available places Cell: {availablePlacesCell.Count}");

        agent = new AStarAgent(new Vector2Int((int) goalPosition.position.x, (int) goalPosition.position.y));
        moves = agent.getMoves(availableCells2D, new Vector2Int((int)startPosition.position.x, (int) startPosition.position.y), tileMap);

        // Debug.Log("Path:");
        // foreach (var move in moves)
        // {
        //     Debug.Log($"Pos: ({move.x}, {move.y})");
        // }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
