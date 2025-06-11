using PlasticGui.Configuration;
using TMPro;
using UnityEngine;
using Viador.Game;

namespace Viador.UI
{
    public class TextController : MonoBehaviour
    {
        public void OnTextOverwrite(Component sender, object payload)
        {
            GetComponent<TMP_Text>().text = payload.ToString();
        }

        public void OnHealthPointsUpdated(Component sender, object payload)
        {
            float healthPoint = float.Parse(payload.ToString());
            if (sender.name == name)
            {
                GetComponent<TMP_Text>().text = healthPoint >= 0.0? healthPoint.ToString() : "0";
            } 
        }

        public void OnGameOverText(Component sender, object payload)
        {
            GetComponent<TMP_Text>().enabled = true;
            GetComponent<TMP_Text>().text = $"The winner is {TurnManager._currentPlayer}";
           
            // Let the game ends, game is freezed
            // TODO: PLAY AGAIN button
            Time.timeScale = 0;
        }
    }
}
