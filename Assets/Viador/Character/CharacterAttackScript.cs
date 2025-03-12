using System;
using UnityEngine;
using Viador.Map;

namespace Viador.Character
{
    public class CharacterAttackScript : MonoBehaviour
    {
        [SerializeField] private GridController _gridController;

        void Awake()
        {
            _gridController = GameObject.Find("Grid").GetComponent<GridController>();

            if (_gridController is null)
            {
                throw new NullReferenceException("No GridController found");
            }
        }

        void Start()
        {
            Debug.Log("Character: Start");
        }

        void Update()
        {
        
        }

        public void OnSelectAttackEvent(Component sender, object targetToHit)
        {
            if (targetToHit is GameObject targetGameObject)
            {
                Debug.Log($"Attack on: {targetToHit}");
                Attack(targetGameObject);
                //HighlightMoveOptions(_gridController.GetComponent<Grid>().CellToWorld(Vector3Int.CeilToInt(sender.transform.position)) + new Vector3(0, 0.25f, 0));
            }
        }

        private void Attack(GameObject targetToAttack)
        {
            Debug.Log($"GridController: {_gridController is not null}");

            Debug.Log($"{targetToAttack.name} has been attacked!");
            targetToAttack.GetComponent<CharacterScript>()._health--;

           // _gridController.ResetHighlight();
        }

        private void HighlightMoveOptions(Vector3 characterPosition)
        {
            _gridController.HighlightMoveOptions(characterPosition);
        }
    }
}
