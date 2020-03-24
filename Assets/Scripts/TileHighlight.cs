using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

using UnityEngine;

public class TileHighlight : MonoBehaviour
{
    public Tilemap tilemap;
    public Sprite highlight;
    Sprite oldSprite;
    Vector3Int oldpos;
    // Start is called before the first frame update
    void Start()
    {
        oldpos = new Vector3Int(-99999, -99999, -9999);
    }

    // Update is called once per frame
    public void CallUpdate()
    {
        // Get mouse position in cellspace
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = tilemap.WorldToCell(worldPoint);
        
        // Check if mouse on new tile on tilemap
        if(tilemap.HasTile(position) && position != oldpos) {     
            // Change last tile to old sprite
            if(tilemap.HasTile(oldpos)) {
                Tile oldTile = ScriptableObject.CreateInstance<Tile>();
                oldTile.sprite = oldSprite;
                tilemap.SetTile(oldpos, oldTile);
            }

            // Register current tile as old tile
            oldpos = position;
            oldSprite = tilemap.GetSprite(position);

            // Change current tile to highlight sprite
            Tile hightlightTile = ScriptableObject.CreateInstance<Tile>();
            hightlightTile.sprite = highlight;
            tilemap.SetTile(position, hightlightTile);

        }
    }
}
