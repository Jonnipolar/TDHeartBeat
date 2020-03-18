using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GetTilePosition : MonoBehaviour
{
    public Tilemap m_level;
    Vector3Int oldpos;
    // Start is called before the first frame update
    void Start()
    {
        m_level = GetComponent<Tilemap>();
        oldpos = new Vector3Int(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = m_level.WorldToCell(worldPoint);
        TileBase tile = m_level.GetTile(position);
        Vector3 world2 = m_level.GetCellCenterWorld(position);
        if(tile && position != oldpos) {
            
            // Debug.Log("World position of mouse: "+ worldPoint);
            // Debug.Log("Cell space position: "+ position);
            // Debug.Log("World pos of cell center "+ world2);
            Debug.Log($"World: {world2} \nCell: {position}");
            oldpos = position;
        }
    }
}
