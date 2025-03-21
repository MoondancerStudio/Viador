using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Events;
using Viador.Util;

namespace Viador.Map
{
    [RequireComponent(typeof(TilemapCollider2D))]
    public class MoveHighlightController : MonoBehaviour
    {
        private static readonly Vector3 RectangleGridOffset = new(0.5f, 0.5f, 0);
        private static readonly Vector3 IsometricGridOffset = new(0, 0.25f, 0);
        private List<Vector3> x;
        public IUnityService UnityService; // Public for testing
        
        [SerializeField] GameEvent selectMoveEvent;
        [SerializeField] private int threshold;
        

        private TilemapCollider2D _tilemapCollider;
        private Grid _grid;
        
        void Awake()
        {
            if (UnityService == null)
            {
                UnityService = new UnityService();
            }
            
            selectMoveEvent = GameEventProvider.Get("MoveSelected");
            
            _grid = UnityService.FindGameObject("Grid").GetComponent<Grid>();
            _tilemapCollider = this.GetComponent<TilemapCollider2D>();
            _tilemapCollider.isTrigger = true;
        }

        public void OnMouseDown()
        {
            Vector2 mousePos = UnityService.GetMousePosition();
            Vector3 worldPos = UnityService.GetMainCamera().ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            Vector3Int tilePos = _grid.WorldToCell(worldPos);
            Debug.Log($"Click on {mousePos}|{worldPos}|{tilePos}");

            if (!_tilemapCollider.OverlapPoint(worldPos))
            {
                Debug.Log("No overlapping tile, no trigger");
                return;
            }
            
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
            bool haveEnoughActionPoints = threshold <= (int) actionPoints;
            EnableTilemapInteractions(haveEnoughActionPoints);
        }

        private void EnableTilemapInteractions(bool value)
        {
            this.gameObject.GetComponent<TilemapRenderer>().enabled = value;
            this.gameObject.GetComponent<TilemapCollider2D>().enabled = value;
        }
        
        public void OnActionPointsUpdated(Component sender, object actionPoints)
        {
            Debug.Log("ActionPointsUpdated: " + actionPoints);
        }
    }
}