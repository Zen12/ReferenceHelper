using UnityEngine;
using UnityEngine.Events;

namespace ReferenceHelper.Sample
{
    public class TestScript : MonoBehaviour
    {
        public Camera Camera;
        public Animation Animation;
        public UnityEvent Event;
        
        [SerializeField] private Camera _camera;

        public GameObject GameObject;
        public Transform Transform;
    }
}
