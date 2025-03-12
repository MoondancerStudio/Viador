using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Viador.Map
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Tile highlightTile;
        [SerializeField] private Tile attackHighlightTile;

        private Tilemap _boardTilemap;
        private Grid _grid;
        private Tilemap _highlightTilemap;
        private Tilemap _obstacleTilemap;

        void Awake()
        {
            _grid = GetComponent<Grid>();
            
            if (_grid == null)
            {
                throw new NullReferenceException("No Grid found");
            }

            _boardTilemap = GameObject.Find("BoardTilemap").GetComponent<Tilemap>();

            if (_boardTilemap == null)
            {
                throw new NullReferenceException("No Board Tilemap found");
            }

            _highlightTilemap = GameObject.Find("HighlightTilemap").GetComponent<Tilemap>();

            if (_highlightTilemap == null)
            {
                throw new NullReferenceException("No Highlight Tilemap found");
            }
            
            _obstacleTilemap = GameObject.Find("ObstacleTilemap").GetComponent<Tilemap>();

            if (_obstacleTilemap == null)
            {
                throw new NullReferenceException("No Obstacle Tilemap found");
            }
        }

        private void OnMouseDown()
        {
            Debug.Log("OnMouseDown");
        }

        public void HighlightMoveOptions(Vector3 characterPosition)
        {
            Debug.Log("Grid: HighlightMoveOptions");
            Vector3Int characterGridPosition = _grid.WorldToCell(characterPosition);
            SetPerimeterTiles(characterGridPosition, highlightTile);
        }

        private void SetPerimeterTiles(Vector3Int charTilePos, Tile highlightTile)
        {
            SetTile(charTilePos + Vector3Int.up + Vector3Int.left, highlightTile);
            SetTile(charTilePos + Vector3Int.up, highlightTile);
            SetTile(charTilePos + Vector3Int.up + Vector3Int.right, highlightTile);
            SetTile(charTilePos + Vector3Int.left, highlightTile);
            SetTile(charTilePos + Vector3Int.right, highlightTile);
            SetTile(charTilePos + Vector3Int.down + Vector3Int.left, highlightTile);
            SetTile(charTilePos + Vector3Int.down, highlightTile);
            SetTile(charTilePos + Vector3Int.down + Vector3Int.right, highlightTile);
        }

        private void SetTile(Vector3Int tileCoordinate, Tile tile)
        {
            if (IsOnBoard(tileCoordinate) && !IsBlocked(tileCoordinate))
            {
                Vector2 tileWorldPosition = _grid.CellToWorld(tileCoordinate);

                if (Physics2D.OverlapBox(tileWorldPosition, new Vector2(0.2f,0.2f), 0, LayerMask.GetMask("Enemy")) is Collider2D targetHit)
                {
               //    Debug.Log($"Click on attack {tileCoordinate}");
                //   targetHit.gameObject.transform.parent = attackHighlightTile.gameObject.transform;
                  _highlightTilemap.SetTile(tileCoordinate, attackHighlightTile);
                }
                else
                {
                  _highlightTilemap.SetTile(tileCoordinate, tile);
                }
            }
        }

        private bool IsBlocked(Vector3Int tileCoordinate)
        {
            return _obstacleTilemap.GetTile(tileCoordinate) is not null;
        }

        private bool IsOnBoard(Vector3Int tileCoordinate)
        {
            return _boardTilemap.GetTile(tileCoordinate) is not null;
        }

        public void ResetHighlight()
        {
            _highlightTilemap.ClearAllTiles();
        }
        
        public void MoveCharacterPositionHighlight(Vector3 characterPosition, Vector3 newPosition)
        {
            Vector3Int oldGridPosition = _grid.WorldToCell(characterPosition);
            Vector3Int newGridPosition = _grid.WorldToCell(newPosition);
            Debug.Log($"Grid: MoveCharacterPositionHighlight {oldGridPosition}; {newGridPosition}");
            
            _obstacleTilemap.SetTile(oldGridPosition, null);
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.color = new Color(12, 56, 147, 0.5f); // FIXME
            _obstacleTilemap.SetTile(newGridPosition, tile);
        }
    }
}