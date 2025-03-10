using UnityEngine;

namespace Viador.Scenes.Sandboxes.EventFlow
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private int threshold = 0;

        public void OnActionPointsUpdated(Component sender, object actionPoints)
        {
            Debug.Log("ActionPointsUpdated: " + actionPoints);
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
