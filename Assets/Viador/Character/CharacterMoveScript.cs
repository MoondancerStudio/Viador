using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Events;
using Viador.GameMechanics;
using Viador.Map;

namespace Viador.Character
{
    public class CharacterMoveScript : MonoBehaviour
    {
        [SerializeField] private GridController _gridController;
        [SerializeField] private bool isSmoothTransitionActive;
        [SerializeField] private int threshold;

        private float smoothSpeed = 5f;
        private bool _isEnabled = false;
        private Vector3 _targetPosition;
        private bool reached;


        void Awake()
        {
            threshold = threshold == 0 ? 1 : threshold;
            _gridController = GameObject.Find("Grid").GetComponent<GridController>();

            if (_gridController is null)
            {
                throw new NullReferenceException("No GridController found");
            }

            _targetPosition = gameObject.transform.position;
            
        }

        private void Start()
        {
            _gridController.MoveCharacterPositionHighlight(_targetPosition, _targetPosition);
        }

        void Update()
        {
            if (reached) return;
   
            if (isSmoothTransitionActive)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    _targetPosition,
                    smoothSpeed * Time.deltaTime
);
            }
            else
            {
                gameObject.transform.position = _targetPosition;               
            }
            

            if (Vector3.Distance(transform.position, _targetPosition) < 0.0001f)
            {
                reached = true;
            }
        }

        public void OnSelectMoveEvent(Component sender, object targetPosition)
        {
            if (!_isEnabled) return;

            if (targetPosition is Vector3 target)
            {
                _gridController.MoveCharacterPositionHighlight(gameObject.transform.position, target);
                Move(target);
                GameEventProvider.Get(GameEvents.CharacterMoved).Trigger(this, null);
            }
        }

        private void Move(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
            reached = false;
            _gridController.ResetHighlight();
        }

        private void HighlightMoveOptions(Vector3 characterPosition)
        {
            _gridController.HighlightMoveOptions(characterPosition);
        }

        public void OnPlayerUpdated(Component sender, object payload)
        {
            _gridController.ResetHighlight();

            if (payload is string)
            {
                if (payload.Equals(gameObject.name))
                {
                    _isEnabled = true;
                }
                else
                {
                    _isEnabled = false;
                }
            }
        }

        public void OnActionPointsUpdated(Component sender, object actionPoints)
        {
            bool haveEnoughActionPoints = threshold <= (int)actionPoints;

            if (haveEnoughActionPoints && _isEnabled)
            {
                HighlightMoveOptions(_targetPosition);
            }
        }

        public void SetPositionForTest(Vector3 position)
        {
            gameObject.transform.position = position;
            _targetPosition = position;
        }
    }
}