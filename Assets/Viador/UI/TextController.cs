using TMPro;
using UnityEngine;

namespace Viador.UI
{
    public class TextController : MonoBehaviour
    {
        public void OnTextOverwrite(Component sender, object payload)
        {
            this.gameObject.GetComponent<TMP_Text>().text = payload.ToString();
        }
    }
}
