using System;
using UnityEngine;
using Viador.Map;

namespace Viador.Character
{
    public class CharacterMoveScript : MonoBehaviour
    {
        [SerializeField] private GridController _gridController;
        [SerializeField] private bool isSmoothTransitionActive;

        private Vector3 _targetPosition;

        void Awake()
        {
            _gridController = GameObject.Find("Grid").GetComponent<GridController>();

            if (_gridController is null)
            {
                throw new NullReferenceException("No GridController found");
            }
            
            _targetPosition = gameObject.transform.position;
        }

        void Start()
        {
            //Debug.Log("Character: Start");
            HighlightMoveOptions(gameObject.transform.position);
        }

        void Update()
        {
            if (Vector3.Distance(gameObject.transform.position, _targetPosition) > 0.1f)
            {
                if (isSmoothTransitionActive)
                {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _targetPosition, Time.deltaTime * 10f);
                }
                else
                {
                    gameObject.transform.position = _targetPosition;
                }
            }
        }
        
        public void OnSelectMoveEvent(Component sender, object targetPosition)
        {
            if (targetPosition is Vector3 target)
            {
                Move(target);
                HighlightMoveOptions(target);
            }
        }

        private void Move(Vector3 targetPosition)
        {
            //Debug.Log($"GridController: {_gridController is not null}");
            _targetPosition = targetPosition;
            _gridController.ResetHighlight();
        }

        private void HighlightMoveOptions(Vector3 characterPosition)
        {
            _gridController.HighlightMoveOptions(characterPosition);
        }
    }
}
