using UnityEngine;
using Viador.Game;

namespace Viador.UI
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private int threshold = 0;

        public void OnActionPointsUpdated(Component sender, object actionPoints)
        {
            GameLogger.Log(LoggerType.ACTION_POINT,"ActionPointsUpdated: " + actionPoints);
            if (threshold <= (int)actionPoints)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
