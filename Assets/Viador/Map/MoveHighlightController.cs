using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Events;

namespace Viador.Map
{
    [RequireComponent(typeof(TilemapCollider2D))]
    public class MoveHighlightController : MonoBehaviour
    {
        [SerializeField] GameEvent selectMoveEvent;
        
        private TilemapCollider2D _tilemapCollider;
        private Grid _grid;

        void Awake()
        {
            _grid = GameObject.Find("Grid").GetComponent<Grid>();
            _tilemapCollider = GetComponent<TilemapCollider2D>();
            _tilemapCollider.isTrigger = true;
        }

        private void OnMouseDown()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            Vector3Int tilePos = _grid.WorldToCell(worldPos);
            Debug.Log($"Click on {tilePos}");
            
            GridLayout.CellLayout cellLayout = _grid.cellLayout;
            Vector3 delta = Vector3.zero;
            if (cellLayout == GridLayout.CellLayout.Rectangle)
            {
                delta = new Vector3(0.5f, 0.5f, 0); // Square
            } else if (cellLayout == GridLayout.CellLayout.Isometric)
            {
                delta = new Vector3(0, 0.25f, 0); // Isometric
            }
            
            selectMoveEvent.Trigger(this, _grid.CellToWorld(tilePos) + delta);
        }
    }
}