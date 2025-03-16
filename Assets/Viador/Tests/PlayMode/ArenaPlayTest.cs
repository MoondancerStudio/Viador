using System.Collections;
using GeneratedAutomationTests;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
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
        
        private System.Collections.Generic.List<Vector2> player1Moves = new()
        {
            new Vector2(954.03f, 343.09f),
            new Vector2(1016.69f, 378.90f),
            new Vector2(1007.74f, 426.63f),
            new Vector2(1052.49f, 462.43f)
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
            // Given
            // WHEN
            yield return ArenaPageObject.Click();
            var actualActionPoints = ArenaPageObject.GetActualActionPoints();
            
            // THEN
            Assert.AreEqual(5, actualActionPoints);
        }

        [UnityTest]
        public IEnumerator OnePlayerMovementTest()
        {
            // Given
            Vector2 newPosition = Camera.main.ScreenToWorldPoint(player1Moves[0]);
            
            // WHEN
            yield return ArenaPageObject.Click(new Vector2(GetNormalizedCoordinateX(player1Moves[0]),GetNormalizedCoordinateY(player1Moves[0])));
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
            Vector2 suggestedPosition = player1Moves[3];
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
            yield return ArenaPageObject.Click();
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
