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
    }
}
