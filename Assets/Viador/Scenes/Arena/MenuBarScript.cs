using UnityEngine;
using UnityEngine.SceneManagement;

namespace Viador.Scenes.Arena
{
    public class MenuBarScript : MonoBehaviour
    {
        public void OnQuitButtonClicked()
        {
            SceneManager.LoadScene("Viador/Scenes/MainMenu");
        }
    }
}
