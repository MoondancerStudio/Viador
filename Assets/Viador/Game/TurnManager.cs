using System;
using System.Collections.Generic;
using UnityEngine;
using Viador.Events;
using Viador.GameMechanics;

namespace Viador.Game
{
    public class TurnManager
    {
        private readonly List<string> _players;
        
        private int _currentTurn;
        private int _actionPoints;

        public static string _currentPlayer;  

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

            // If the attack is on-going, then it costs 2 to move away, otherwise 1
            if (CombatLogic.IsAttackBegin)
            {
                CombatLogic.IsAttackBegin = false;
                _actionPoints -= 2;
            }

            Debug.Log("Action points after move:" + _actionPoints);
            GameEventProvider.Get(GameEvents.ActionPointsUpdated).Trigger(null, _actionPoints);
        }

        public void OnAttacked()
        {
            if (_currentTurn < 0)
            {
                throw new InvalidOperationException("Cannot attack when there is no action point");
            }

            _actionPoints -= 2;
       
            Debug.Log("Action points after attack:" + _actionPoints);
            GameEventProvider.Get(GameEvents.ActionPointsUpdated).Trigger(null, _actionPoints);
        }
    }
}