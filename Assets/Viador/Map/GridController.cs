using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Viador.Character;

namespace Viador.Map
{
    public class GridController : MonoBehaviour
    {
        private static readonly Vector2 sizeOfBoxCollider = new(0.5f, 0.5f);

        [SerializeField] private Tile highlightTile;
        [SerializeField] private Tile attackHighlightTile;

        private Tilemap _boardTilemap;
        private Grid _grid;
        private Tilemap _MovehighlightTilemap;
        private Tilemap _AttackhighlightTilemap;
        private Tilemap _obstacleTilemap;

        Vector2 halfSize = Vector2.one;

        Vector3 topLeft = Vector2.one;
        Vector3 topRight = Vector2.one;
        Vector3 bottomLeft = Vector2.one;
        Vector3 bottomRight = Vector2.one;

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

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.color = Color.yellow;
        }

        void DrawOverlapBox(Vector2 center, Vector2 size)
        {
            Vector2 halfSize = size;

             topLeft = center + new Vector2(-halfSize.x, halfSize.y);
             topRight = center + new Vector2(halfSize.x, halfSize.y);
             bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);
             bottomRight = center + new Vector2(halfSize.x, -halfSize.y);
        }

        private void SetTile(Vector3Int tileCoordinate, Tile tile)
        {
            if (IsOnBoard(tileCoordinate) && !IsBlocked(tileCoordinate))
            {
                Vector2 tileWorldPosition = _grid.CellToWorld(tileCoordinate);


             //   tileWorldPosition.Set(tileCoordinate.x, tileCoordinate.y);

                if (Physics2D.OverlapBox(tileWorldPosition, sizeOfBoxCollider, 0, LayerMask.GetMask("Character")) is Collider2D enemy)
                {
                    DrawOverlapBox(tileWorldPosition, sizeOfBoxCollider);
                    Debug.Log($"Character name: {enemy.name} with pos: {enemy.transform.position} standing at {tileWorldPosition}");
               //     enemy.transform.position = _grid.WorldToCell(enemy.transform.position);
                //    tileCoordinate = new Vector3Int((int)enemy.transform.position.x, (int)enemy.transform.position.y);
               //     _AttackhighlightTilemap.SetTile(new Vector3Int((int)enemy.transform.position.x, (int)enemy.transform.position.y), attackHighlightTile);
                   _AttackhighlightTilemap.SetTile(tileCoordinate, attackHighlightTile);
                }
                else
                {
                  _MovehighlightTilemap.SetTile(tileCoordinate, tile);
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