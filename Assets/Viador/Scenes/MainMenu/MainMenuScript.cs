using UnityEngine;
using UnityEngine.SceneManagement;

namespace Viador.Scenes.MainMenu
{
    public class MainMenuScript : MonoBehaviour
    {
        public void OnStartButtonClicked()
        {
            SceneManager.LoadScene("Viador/Scenes/Arena");
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}
