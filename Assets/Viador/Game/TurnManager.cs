using System;
using System.Collections.Generic;
using Viador.Events;

namespace Viador.Game
{
    public class TurnManager
    {
        private readonly List<string> _players;
        
        private int _currentTurn;
        private string _currentPlayer;
        
        private int _actionPoints;

        public TurnManager(List<string> players)
        {
            _players = players;
            _currentTurn = 0;
            _currentPlayer = _players[0];
        }

        public void OnNextTurn()
        {
            var indexOfPlayer = _players.IndexOf(_currentPlayer);
            _actionPoints = 6;

            if (_currentTurn == 0)
            {
                _currentTurn = 1;
                GameEventProvider.Get(GameEvents.TurnStarted).Trigger(null, _currentTurn);
            }
            else if (indexOfPlayer == _players.Count - 1)
            {
                _currentTurn++;
                _currentPlayer = _players[0];
                
                GameEventProvider.Get(GameEvents.TurnStarted).Trigger(null, _currentTurn);
            }
            else
            {
                _currentPlayer = _players[indexOfPlayer + 1];
            }
            
            GameEventProvider.Get(GameEvents.PlayerUpdated).Trigger(null, _currentPlayer);
            GameEventProvider.Get(GameEvents.ActionPointsUpdated).Trigger(null, _actionPoints);
        }

        public void OnMoved()
        {
            if (_currentTurn < 0)
            {
                throw new InvalidOperationException("Cannot move when there is no action point");
            }
            
            _actionPoints -= 1;
            GameEventProvider.Get(GameEvents.ActionPointsUpdated).Trigger(null, _actionPoints);
        }
    }
}