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
            if (IsOnBoard(tileCoordinate))
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

        private bool IsOnBoard(Vector3Int tileCoordinate)
        {
            return _boardTilemap.GetTile(tileCoordinate) is not null;
        }

        public void ResetHighlight()
        {
            _highlightTilemap.ClearAllTiles();
        }
    }
}