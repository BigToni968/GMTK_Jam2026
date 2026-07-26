using UnityEngine;

namespace Game.Component
{
    public class RotateCamera : MonoBehaviour
    {
        [SerializeField] private Vector2 VClamp = new(-60f, 60f);

        private float _rotX;

        public void Rotate(Transform body, Vector2 sensitivity)
        {
            var mouseX = Input.GetAxis("Mouse X") * sensitivity.x;
            var mouseY = Input.GetAxis("Mouse Y") * sensitivity.y;
            
            body.Rotate(Vector3.up * mouseX);
            
            _rotX -= mouseY;
            _rotX = Mathf.Clamp(_rotX, VClamp.x, VClamp.y);
            
            transform.localRotation = Quaternion.Euler(_rotX, 0f, 0f);
        }
    }
}