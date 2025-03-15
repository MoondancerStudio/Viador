using UnityEngine;
using UnityEngine.EventSystems;
using Viador.Util;

namespace Viador.Tests.PlayMode.Mocks
{
    public class RecordableUnityService : IUnityService
    {
        private Vector3 _mousePosition;

        public Camera GetMainCamera()
        {
            return Camera.main;
        }

        public GameObject FindGameObject(string name)
        {
            return GameObject.Find(name);
        }

        public Vector3 GetMousePosition()
        {
            return RecordableInput.mousePosition;
        }
    }
}