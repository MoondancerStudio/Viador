using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Character;

namespace Viador.Map
{
    public class GridController : MonoBehaviour
    {
        private static readonly Vector2 sizeOfBoxCollider = new(0.2f, 0.2f);

        [SerializeField] private Tile highlightTile;
        [SerializeField] private Tile attackHighlightTile;

        private Tilemap _boardTilemap;
        private Grid _grid;
        private Tilemap _MovehighlightTilemap;
        private Tilemap _AttackhighlightTilemap;

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

            _MovehighlightTilemap = GameObject.Find("MoveHighlightTilemap").GetComponent<Tilemap>();

            if (_MovehighlightTilemap == null)
            {
                throw new NullReferenceException("No Highlight Tilemap found");
            }

            _AttackhighlightTilemap = GameObject.Find("AttackHighlightTilemap").GetComponent<Tilemap>();

            if (_AttackhighlightTilemap == null)
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

                if (Physics2D.OverlapBox(tileWorldPosition, sizeOfBoxCollider, 0, LayerMask.GetMask("Character")) is Collider2D targetHit)
                {
                  _AttackhighlightTilemap.SetTile(tileCoordinate, attackHighlightTile);
                }
                else
                {
                  _MovehighlightTilemap.SetTile(tileCoordinate, tile);
                }
            }
        }

        private bool IsOnBoard(Vector3Int tileCoordinate)
        {
            return _boardTilemap.GetTile(tileCoordinate) is not null;
        }

        public void ResetHighlight()
        {
            _MovehighlightTilemap.ClearAllTiles();
            _AttackhighlightTilemap.ClearAllTiles();
        }
    }
}