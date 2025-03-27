using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Events;

namespace Viador.Map
{
    [RequireComponent(typeof(TilemapCollider2D))]
    public class AttackHighlightController : MonoBehaviour
    {
        [SerializeField] GameEvent selectAttackEvent;
        [SerializeField] LayerMask layerIncluded;
        [SerializeField] private int threshold;

        private TilemapCollider2D _tilemapCollider;

        void Awake()
        {
            threshold = threshold == 0 ? 1 : threshold;
            _tilemapCollider = GetComponent<TilemapCollider2D>();
            _tilemapCollider.isTrigger = true;
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