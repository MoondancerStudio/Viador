using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Events;
using Viador.Game;
using Viador.Util;

namespace Viador.Map
{
    [RequireComponent(typeof(TilemapCollider2D))]
    public class MoveHighlightController : MonoBehaviour
    {
        private static readonly Vector3 RectangleGridOffset = new(0.5f, 0.5f, 0);
        private static readonly Vector3 IsometricGridOffset = new(0, 0.25f, 0);
     
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

        private Vector3 getDelta(Grid grid)
        {
            GridLayout.CellLayout cellLayout = grid.cellLayout;
            Vector3 delta = Vector3.zero;
            if (cellLayout == GridLayout.CellLayout.Rectangle)
            {
                delta = RectangleGridOffset;
            }
            else if (cellLayout == GridLayout.CellLayout.Isometric)
            {
                delta = IsometricGridOffset;
            }
            return delta;
        }

        public void OnMouseDown()
        {
            Vector2 mousePos = UnityService.GetMousePosition();
            Vector3 worldPos = UnityService.GetMainCamera().ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            Vector3Int tilePos = _grid.WorldToCell(worldPos);
            GameLogger.Log(LoggerType.GAME_INFO,$"Click on {mousePos}|{worldPos}|{tilePos}");

            if (!_tilemapCollider.OverlapPoint(worldPos))
            {
                GameLogger.Log(LoggerType.EVENTS,"No overlapping tile, no trigger");
                return;
            }

            selectMoveEvent.Trigger(this, _grid.CellToWorld(tilePos) + getDelta(_grid));       
        }
        
        public void OnActionPointsUpdated(Component sender, object actionPoints)
        {
            GameLogger.Log(LoggerType.ACTION_POINT,"ActionPointsUpdated: " + actionPoints);
            bool haveEnoughActionPoints = threshold <= (int) actionPoints;
            EnableTilemapInteractions(haveEnoughActionPoints);
        }

        private void EnableTilemapInteractions(bool value)
        {
            GetComponent<TilemapRenderer>().enabled = value;
            GetComponent<TilemapCollider2D>().enabled = value;
        }
    }
}