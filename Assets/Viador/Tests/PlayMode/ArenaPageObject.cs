using System;
using System.Collections;
using TMPro;
using Unity.AutomatedQA;
using UnityEngine;
using UnityEngine.SceneManagement;
using Viador.Map;

namespace Viador.Tests.PlayMode
{
    public class ArenaPageObject
    {
        public const string SceneName = "Arena";
        private const string RoundValue = "UI/MenuBar/Round/Text";
        private const string ActionPointsValue = "UI/MenuBar/ActionPoints/Text";
        private const string PlayerName = "UI/MenuBar/TurnHighlight/Text";
        private const string NextTurnButton = "UI/MenuBar/TurnHighlight/Button";
        private const string QuitButton = "UI/MenuBar/QuitButton";

        public const string MoveHighlightLayer = "Board/StageContainer/Grid/HighlightTilemap";

        public int GetActualRound()
        {
            var textValue = GameObject.Find(RoundValue).GetComponent<TMP_Text>().text;
            var actualActionPoints = Convert.ToInt32(textValue);
            return actualActionPoints;
        }

        public int GetActualActionPoints()
        {
            var textValue = GameObject.Find(ActionPointsValue).GetComponent<TMP_Text>().text;
            int actualActionPoints = Convert.ToInt32(textValue);
            return actualActionPoints;
        }
        
        public string GetActualPlayerName()
        {
            return GameObject.Find(PlayerName).GetComponent<TMP_Text>().text;
        }

        public IEnumerator NextTurn()
        {
            return Driver.Perform.Click(NextTurnButton);
        }
        
        public IEnumerator Click()
        {
            // TODO: Move mouse in position (Mouse.WarpCursorPosition)
            GameObject.Find(MoveHighlightLayer).GetComponent<MoveHighlightController>().OnMouseDown();
            yield return new WaitForSeconds(0.1f);
        }

        public IEnumerator Click(Vector2 position)
        {
            yield return Driver.Perform.Click(position);
            Debug.Log("Driver clicked");
            yield return Click();
        }

        public IEnumerator Quit()
        {
            return Driver.Perform.Click(QuitButton);
        }
    }
}