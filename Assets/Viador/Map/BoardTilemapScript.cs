using System;
using System.Collections.Generic;
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
        public Tilemap obstacleTilemap;

        public int moveRadius = 1;

        public List<Color> colors;
    
        private bool _isSet = false;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
        }

        void Update()
        {
            if (!_isSet) {
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
            //Debug.Log($"Click on {tilePos}");

            if (IsObstacle(tilePos))
            {
                return;
            }
            
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
            IteratePerimeterTiles(charTilePos, tile, moveRadius, moveRadius);
        }

        private void IteratePerimeterTiles(Vector3Int charTilePos, Tile tile, int x, int y)
        {
            for (int i = 1; i <= y; i++)
            {
                Tile usedTile = null;
                if (tile is not null)
                {
                    usedTile = Instantiate(tile);
                    usedTile.color = colors[i - 1];
                }
                
                SetTile(charTilePos + i * Vector3Int.up, usedTile);
                SetTile(charTilePos + i * Vector3Int.down, usedTile);
                SetTile(charTilePos + i * Vector3Int.right, usedTile);
                SetTile(charTilePos + i * Vector3Int.left, usedTile);
                
                for (int j = 1; j <= x; j++)
                {
                    if (tile is not null)
                    {
                        usedTile.color = colors[Math.Max(i, j) - 1];
                    }
                    
                    SetTile(charTilePos + i * Vector3Int.up + j * Vector3Int.right, usedTile);
                    SetTile(charTilePos + i * Vector3Int.up + j * Vector3Int.left, usedTile);
                    
                    SetTile(charTilePos + i * Vector3Int.down + j * Vector3Int.right, usedTile);
                    SetTile(charTilePos + i * Vector3Int.down + j * Vector3Int.left, usedTile);
                }
                
                
            }
        }

        private void SetTile(Vector3Int tileCoordinate, Tile tile)
        {
            if (IsOnBoard(tileCoordinate) && !IsObstacle(tileCoordinate))
            {
                highlightTilemap.SetTile(tileCoordinate, tile);
            }
        }

        private bool IsOnBoard(Vector3Int tileCoordinate)
        {
            return tilemap.GetTile(tileCoordinate) is not null;
        }
        
        private bool IsObstacle(Vector3Int tileCoordinate)
        {
            return obstacleTilemap is not null && obstacleTilemap.GetTile(tileCoordinate) is not null;
        }
    }
}
