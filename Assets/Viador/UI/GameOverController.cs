using PlasticGui.Configuration;
using TMPro;
using UnityEngine;
using Viador.Game;

namespace Viador.UI
{
    public class GameOverController : MonoBehaviour
    {
        public void OnGameOver(Component sender, object payload)
        {
            transform.Find("Panel").gameObject.SetActive(true);
        }
    }
}
