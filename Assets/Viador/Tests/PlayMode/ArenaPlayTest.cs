using System.Collections;
using GeneratedAutomationTests;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Viador.Character;
using Viador.Map;
using Viador.Tests.PlayMode.Mocks;
using Assert = UnityEngine.Assertions.Assert;

namespace Viador.Tests.PlayMode
{
    public class ArenaPlayTest : AutomatedQATestsBase
    {
        private ArenaPageObject ArenaPageObject;
        private float width = 1920f;
        private float height = 1080f;
        
        private System.Collections.Generic.List<Vector2> playerMoves = new()
        {
            new Vector2(954.03f, 343.09f),
            new Vector2(1016.69f, 378.90f),
            new Vector2(1007.74f, 426.63f),
            new Vector2(1052.49f, 462.43f),
            new Vector2(1073.37f, 498.23f),
            new Vector2(1088.29f, 572.82f),
            
            new Vector2(965.97f, 724.97f), // TODO
            new Vector2(957.02f, 680.22f), // TODO
            new Vector2(951.05f, 608.62f), // TODO
            new Vector2(1010.72f, 590.72f), // TODO
        };

        [SetUp]
        public void Setup()
        {
            ArenaPageObject = new ArenaPageObject();
            
            var mockUnityService = new RecordableUnityService();
            GameObject.Find(ArenaPageObject.MoveHighlightLayer).GetComponent<MoveHighlightController>().UnityService =
                mockUnityService;
        } 
        
        protected override void SetUpTestRun()
        {
            Test.entryScene = ArenaPageObject.SceneName;
            Test.recordedAspectRatio = new Vector2(321f,573f);
            Test.recordedResolution = new Vector2(width, height);
        }

        [UnityTest]
        public IEnumerator FirstTurnNumberTest()
        {
            var actualRound = ArenaPageObject.GetActualRound();
            Assert.AreEqual(1, actualRound);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator FirstTurnPlayerTest()
        {
            // GIVEN
            // WHEN
            var actualPlayerName = ArenaPageObject.GetActualPlayerName();
            
            // THEN
            Assert.AreEqual("Dracon", actualPlayerName);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator InitialActionPointsTest()
        {
            // GIVEN
            // WHEN
            var actualActionPoints = ArenaPageObject.GetActualActionPoints();
            
            // THEN
            Assert.AreEqual(6, actualActionPoints);

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator ActionPointsAfterOneMoveTest()
        {
            // GIVEN
            // WHEN
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[0]));
            yield return null;
            var actualActionPoints = ArenaPageObject.GetActualActionPoints();
            
            // THEN
            Assert.AreEqual(5, actualActionPoints);
        }

        [UnityTest]
        public IEnumerator OnePlayerMovementTest()
        {
            // Given
            Vector2 newPosition = Camera.main.ScreenToWorldPoint(playerMoves[0]);
            
            // WHEN
            yield return ArenaPageObject.Click(new Vector2(GetNormalizedCoordinateX(playerMoves[0]),GetNormalizedCoordinateY(playerMoves[0])));
            var player1Position = GameObject.Find("Dracon").transform.position;
            
            // THEN
            Assert.AreApproximatelyEqual(newPosition.x, player1Position.x, 0.1f, "Player 1 should move (X)");
            Assert.AreApproximatelyEqual(newPosition.y, player1Position.y, 0.1f, "Player 1 should move (Y)");
        }

        [UnityTest]
        public IEnumerator InvalidMovementTest()
        {
            // Given
            Vector3 originalPosition = GameObject.Find("Dracon").transform.position;
            Vector2 suggestedPosition = playerMoves[3];
            Vector2 newPosition = Camera.main.ScreenToWorldPoint(suggestedPosition);
            
            // WHEN
            yield return ArenaPageObject.Click(new Vector2(GetNormalizedCoordinateX(suggestedPosition),GetNormalizedCoordinateY(suggestedPosition)));
            var player1Position = GameObject.Find("Dracon").transform.position;
            
            // THEN
            Assert.AreEqual(originalPosition, player1Position, "Player should not move to invalid tile");
        }

        [UnityTest]
        public IEnumerator NextPlayerTest()
        {
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[0]));
            yield return null;
            Assert.AreEqual(5, ArenaPageObject.GetActualActionPoints());
            yield return ArenaPageObject.NextTurn();
            var actualRound = ArenaPageObject.GetActualRound();
            Assert.AreEqual(1, actualRound, "Turn should not change");
            var actualPlayerName = ArenaPageObject.GetActualPlayerName();
            Assert.AreEqual("Quicchures", actualPlayerName, "Player name should change");
            var actualActionPoints = ArenaPageObject.GetActualActionPoints();
            Assert.AreEqual(6, actualActionPoints, "Action points should reset");
        }
        
        [UnityTest]
        public IEnumerator NextTurnnTest()
        {
            yield return ArenaPageObject.NextTurn();
            yield return ArenaPageObject.NextTurn();
            
            Assert.AreEqual(2, ArenaPageObject.GetActualRound(), "Turn should change");
            Assert.AreEqual("Dracon", ArenaPageObject.GetActualPlayerName(), "Player is first player");
        }

        [UnityTest]
        public IEnumerator QuitTest()
        {
            yield return ArenaPageObject.Quit();
            var actualSceneName = SceneManager.GetActiveScene().name;
            Assert.AreNotEqual(ArenaPageObject.SceneName, actualSceneName);
        }
        
        [UnityTest]
        public IEnumerator X()
        {
            // GIVEN
            var camera = Camera.main;
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[0]));
            Debug.Log($"Test Clicked {playerMoves[0]}");
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[1]));
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[2]));
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[3]));
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[4]));
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[5]));
            yield return ArenaPageObject.NextTurn();
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[6]));
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[7]));
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[8]));
            var lastValidMove = ToScreenProportionate(playerMoves[9]);
            yield return ArenaPageObject.Click(lastValidMove);
            
            // WHEN
            yield return ArenaPageObject.Click(ToScreenProportionate(playerMoves[5]));
            
            // THEN

            var player1Position = GameObject.Find("Quicchures").transform.position;
            Assert.AreApproximatelyEqual(lastValidMove.x, player1Position.x, 0.1f, "Player 2 should move (X)");
            Assert.AreApproximatelyEqual(lastValidMove.y, player1Position.y, 0.1f, "Player 2 should move (Y)");
        }

        private Vector2 ToScreenProportionate(Vector3 position)
        {
            return new Vector2(GetNormalizedCoordinateX(position), GetNormalizedCoordinateY(position));
        }
        
        private float GetNormalizedCoordinateY(Vector2 position)
        {
            return position.y / height;
        }

        private float GetNormalizedCoordinateX(Vector2 position)
        {
            return position.x / width;
        }
    }
}
