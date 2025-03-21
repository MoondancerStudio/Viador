using System;
using UnityEngine;
using Viador.Events;
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

        public void OnSelectAttackEvent(Component sender, object targetToHit)
        {
            Debug.Log($"{sender.gameObject.tag} : {targetToHit}");

            if (targetToHit is CharacterData target)
            {
                Debug.Log($"Attack on: {targetToHit}");
                target.health--;
            }

            if (targetToHit is GameObject targetGameObject)
            {
                Attack(targetGameObject);
            }
        }

        private void Attack(GameObject targetToAttack)
        {
            Debug.Log($"GridController: {_gridController is not null}");

            Debug.Log($"{targetToAttack.name} has been attacked!");

          //  GetComponent<CharacterData>().attack
          //  GameEventProvider.Get(GameEvents.CharacterAttacked).Trigger(null, 0);
            //  attackCalculate.trigger(this, damageScore);
        }
    }
}
