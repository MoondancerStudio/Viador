using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Viador.Map
{
    public class BoardTilemapScript : MonoBehaviour
    {
        public Grid grid;
        public GameObject characterObject;
        public Tile highlightTile;
        public Tilemap highlightTilemap;
        public Tilemap tilemap;
    
        private bool _isSet = false;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
        }

        void Update()
        {
            if (!_isSet) {
                Debug.Log("Setting move highlight");
                Vector3Int charTilePos = grid.WorldToCell(characterObject.transform.position);
                
                SetPerimeterTiles(charTilePos, highlightTile);

                _isSet = true;
            }
        }

        void OnMouseDown()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            Vector3Int tilePos = grid.WorldToCell(worldPos);
            Debug.Log($"Click on {tilePos}");
            SetPerimeterTiles(grid.WorldToCell(characterObject.transform.position), null);

            GridLayout.CellLayout cellLayout = tilemap.GetComponentInParent<Grid>().cellLayout;
            if (cellLayout == GridLayout.CellLayout.Rectangle)
            {
                characterObject.transform.position = tilePos + new Vector3(0.5f, 0.5f, 0); // Square
            } else if (cellLayout == GridLayout.CellLayout.Isometric)
            {
                characterObject.transform.position = grid.CellToWorld(tilePos) + new Vector3(0, 0.25f, 0); // Isometric
            }
            
            _isSet = false;
        }
        
        
        private void SetPerimeterTiles(Vector3Int charTilePos, Tile tile)
        {
            SetTile(charTilePos + Vector3Int.up, tile);
            SetTile(charTilePos + Vector3Int.up + Vector3Int.left, tile);
            SetTile(charTilePos + Vector3Int.up + Vector3Int.right, tile);
            SetTile(charTilePos + Vector3Int.left, tile);
            SetTile(charTilePos + Vector3Int.right, tile);
            SetTile(charTilePos + Vector3Int.down, tile);
            SetTile(charTilePos + Vector3Int.down + Vector3Int.left, tile);
            SetTile(charTilePos + Vector3Int.down + Vector3Int.right, tile);
        }

        private void SetTile(Vector3Int tileCoordinate, Tile tile)
        {
            if (tilemap.GetTile(tileCoordinate) != null)
            {
                Debug.Log(tileCoordinate);
                highlightTilemap.SetTile(tileCoordinate, tile);
            }
        }
    }
}
