using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Viador.Character;
using Viador.Game;

namespace Viador.Map
{
    public class GridController : MonoBehaviour
    {
        private static readonly Vector2 sizeOfBoxCollider = new(0.15f, 0.15f);
        private LayerMask _includeLayer;

        [SerializeField] private Tile highlightTile;
        [SerializeField] private Tile attackHighlightTile;

        private Tilemap _boardTilemap;
        private Grid _grid;
        private Tilemap _MovehighlightTilemap;
        private Tilemap _AttackhighlightTilemap;
        private Tilemap _obstacleTilemap;

        void Awake()
        {
            _grid = GetComponent<Grid>();
            _includeLayer = LayerMask.GetMask("Character");

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
            Debug.Log($"tile original pos {_grid.CellToWorld(charTilePos)}");

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
            Vector2 tileWorldPosition = _grid.GetCellCenterWorld(tileCoordinate);

            if (IsOnBoard(tileCoordinate))
            {
                if (!IsBlocked(tileCoordinate))
                { 
                    _MovehighlightTilemap.SetTile(tileCoordinate, tile);
                }
                else // It is a blocked tile, must be an enemy there...
                {
                    if (Physics2D.OverlapBox(tileWorldPosition, sizeOfBoxCollider, 0, _includeLayer) is Collider2D character)
                    {
                        if (TurnManager._currentPlayer != character.name)
                        {
                            Debug.Log($"{_grid.WorldToCell(tileWorldPosition)} = {tileCoordinate}");
                            Debug.Log($"Character name: {character.name} with pos: {character.transform.position}");

                            _AttackhighlightTilemap.SetTile(tileCoordinate, attackHighlightTile);
                        }
                    }
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
            _MovehighlightTilemap.ClearAllTiles();
            _AttackhighlightTilemap.ClearAllTiles();
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