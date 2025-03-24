using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Events;

namespace Viador.Map
{
    [RequireComponent(typeof(TilemapCollider2D))]
    public class AttackHighlightController : MonoBehaviour
    {
        private static readonly Vector2 sizeOfBoxCollider = new(0.2f, 0.2f);

        [SerializeField] GameEvent selectAttackEvent;
        [SerializeField] LayerMask layerIncluded;
        [SerializeField] private int threshold;

        private TilemapCollider2D _tilemapCollider;
        private Grid _grid;

        void Awake()
        {
            threshold = threshold == 0 ? 1 : threshold;
            _grid = GameObject.Find("Grid").GetComponent<Grid>();
            _tilemapCollider = GetComponent<TilemapCollider2D>();
            _tilemapCollider.isTrigger = true;
        }

        private void OnMouseDown()
        {
            /*
            Vector2 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            Vector3Int tilePos = _grid.WorldToCell(worldPos);
            Debug.Log($"Click on {tilePos}");

            if (Physics2D.OverlapBox(worldPos, sizeOfBoxCollider, 0, layerIncluded) is Collider2D targetHit)
            {
                if (targetHit != null)
                {
                    Debug.Log($"Start attack {targetHit.name.ToString()}");

                    //TODO: get player attack score
                    //   selectAttackEvent.Trigger(this, 1);
                }
            }
            */
        }

        public void OnActionPointsUpdatedForAttack(Component sender, object actionPoints)
        {
            Debug.Log("ActionPointsUpdated for attack: " + actionPoints);
            bool haveEnoughActionPoints = threshold <= (int)actionPoints;
            EnableTilemapInteractionsForAttack(haveEnoughActionPoints);
        }

        private void EnableTilemapInteractionsForAttack(bool value)
        {
            GetComponent<TilemapRenderer>().enabled = value;
            GetComponent<TilemapCollider2D>().enabled = value;
        }
    }
}