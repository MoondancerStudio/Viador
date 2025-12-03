
using Codice.CM.Common;
using System.Linq;
using UnityEngine;
using Viador.Character;

namespace Viador.Game
{
    public enum LoggerType
    {
        ATTACK,
        MOVE,
        TURN,
        GAME_OVER,
        GAME_BEGIN,
        ERROR,
        UI_UPDATE,
        FEAT_ACTIVATED
    }

   public interface IGamelogger
    {
        void Log(LoggerType logType, params object[] args);
    }

    //      Debug.Log($"ROUND {round}")
    //      PLAYER TURN: [{current}] move from to
    //      current player attack on {player2}
            // [Attack] 
    // [UI UPDATE]

    public class GameLogger : IGamelogger
    {
        private static GameLogger _instance;
        public static GameLogger Instance => _instance ??= new GameLogger();

        private int logCount = 0;

        private GameLogger() { }

        public void Log(LoggerType logType, params object[] args)
        {

            string msg = string.Join(" ", args.Select(a => a.ToString()));

            Debug.Log($"Current player: [{TurnManager._currentPlayer}] : [{logType.ToString()}]");

            // If a logging is attack type, then it has different schema
            if(logType == LoggerType.ATTACK)
            {
                Debug.Log($"Player 1: [all the stuff]");
                Debug.Log($"Player 2: [all the stuff]");
                // Debug.Log($"Start attack on {name}");
                // Debug.Log($"{name} received {damage} effective damage");
                // Debug.Log($"{name} has {characterData.health} health");
                // Debug.Log($"{name} new health {characterData.health}");
                // Debug.Log($"{name} attacked successfully with a raw damage of {attackResult.Damage}");
                // Debug.Log($"{name} missed the attack");
                Debug.Log($"[{logType}] {msg}");

            }
        }
    }
}
