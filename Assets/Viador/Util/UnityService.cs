using UnityEngine;

namespace Viador.Util
{
    public class UnityService: IUnityService 
    {
        public Camera GetMainCamera()
        {
            return Camera.main;
        }

        public GameObject FindGameObject(string name)
        {
            return GameObject.Find("Grid");
        }

        public Vector3 GetMousePosition()
        {
            return Input.mousePosition;
        }
    }

    public interface IUnityService
    {
        Camera GetMainCamera();
        GameObject FindGameObject(string name);
        Vector3 GetMousePosition();
    }
}