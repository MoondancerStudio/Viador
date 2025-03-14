using NUnit.Framework;
using UnityEngine.SceneManagement;

namespace Viador.Tests
{
    public class AbstractPlayModeTest
    {
        private readonly string _sceneName;
        private bool _sceneLoaded;
        
        public AbstractPlayModeTest(string sceneName)
        {
            _sceneName = sceneName;
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _sceneLoaded = true;
        }

        public bool SceneLoaded
        {
            get => _sceneLoaded;
        }
    }
}