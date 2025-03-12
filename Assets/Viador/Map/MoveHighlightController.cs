using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Events;

namespace Viador.Map
{
    [RequireComponent(typeof(TilemapCollider2D))]
    public class MoveHighlightController : MonoBehaviour
    {
        private static readonly Vector3 RectangleGridOffset = new(0.5f, 0.5f, 0);
        private static readonly Vector3 IsometricGridOffset = new(0, 0.25f, 0);
        
        [SerializeField] GameEvent selectMoveEvent;
        
        private TilemapCollider2D _tilemapCollider;
        private Grid _grid;

        void Awake()
        {
            selectMoveEvent = GameEventProvider.Get("MoveSelected");
            
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
                delta = RectangleGridOffset;
            } else if (cellLayout == GridLayout.CellLayout.Isometric)
            {
                delta = IsometricGridOffset;
            }
            
            selectMoveEvent.Trigger(this, _grid.CellToWorld(tilePos) + delta);
        }
        
        public void OnActionPointsUpdated(Component sender, object actionPoints)
        {
            Debug.Log("ActionPointsUpdated: " + actionPoints);
        }
    }
}