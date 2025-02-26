using System.Collections.Generic;
using UnityEngine;
using Viador.Events;

namespace Viador.GameMechanics
{
    public class Turn : MonoBehaviour
    {
        [SerializeField] private GameEvent turnChanged;
        [SerializeField] private List<string> players;
        [SerializeField] private string activePlayer;
        
        public void OnTurnStart(Component sender, object data)
        {
            
        }

        public void OnTurnEnd(Component sender, object data)
        {
            
        }
    }
}
