using NUnit.Framework.Api;
using System.Linq;
using UnityEngine;

namespace Viador.Game
{
    public enum LoggerType
    {
        GAME_INFO,
        MOVE,
        ATTACK,
        UI,
        FEATS,
        EVENTS,
        TURN,
        GAME_LOOP,
        COMBAT,
        ACTION_POINT
    }

    public class GameLogger
    {
        public static void Log(LoggerType logType, params object[] args)
        {
            string msg = string.Join(" ", args.Select(a => a.ToString()));

            // By default need to print out the current game state to search log properly
            string finalLog = $"[{logType.ToString()}]: \n";
            finalLog += $"[CURRENT PLAYER]: [{TurnManager._currentPlayer}]\n";


            // If a logging is attack type, then it has different schema
            switch (logType)
            {
                case LoggerType.ATTACK:
                    foreach (var characterData in GameObject.FindAnyObjectByType<GameController>().CharacterData)   
                    {
                        finalLog += $"[Player Name:]: {characterData.name}\n" +
                                  $"    [Player health]: {characterData.health}\n" +
                                  $"    [Attack power]: {characterData.attack} \n" +
                                  $"    [Attack defense]: {characterData.defense}\n";
                    }
                    break;
                default:
                    break;

            }
            finalLog += msg;
            Debug.Log(finalLog);
        }
    }
}
