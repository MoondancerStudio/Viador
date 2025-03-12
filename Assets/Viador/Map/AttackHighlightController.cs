using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Events;

namespace Viador.Map
{
    [RequireComponent(typeof(TilemapCollider2D))]
    public class AttackHighlightController : MonoBehaviour
    {
        [SerializeField] GameEvent selectAttackEvent;
        [SerializeField] LayerMask layerIgnored;

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
            //    Debug.Log($"Click on {tilePos}");

            if (Physics2D.OverlapBox(worldPos, new Vector2(0.5f, 0.5f), 0, layerIgnored) is Collider2D hit)
            {
                if (hit != null)
                {
                    Debug.Log($"attack! {hit.gameObject.tag}");
                    
                    selectAttackEvent.Trigger(this, hit.gameObject);
                }
            }
        }
    }
}