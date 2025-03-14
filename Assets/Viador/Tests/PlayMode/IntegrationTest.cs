using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Viador.Tests.PlayMode
{
    public class IntegrationTest : AbstractPlayModeTest
    {
       
        public IntegrationTest() : base("Arena") { }

        [UnityTest]
        public IEnumerator CharacterIdentity([Values("Dracon", "Quicchures")] string characterName)
        {
            // Given
            // WHEN
            yield return new WaitWhile(() => SceneLoaded == false);
            
            // THEN
            Assert.IsNotNull(GameObject.Find(characterName));

            yield return null;
        }
    }
}
