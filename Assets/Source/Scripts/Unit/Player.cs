using Game.Unit.Component;
using Game.Abstraction;
using Game.Component;
using UnityEngine;

namespace Game.Unit
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private RotateCamera  rotateCamera;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float rayDistance;
        [SerializeField] private Vector3 size;
        [SerializeField] private Color colorGizmos;

        [field: SerializeField] public Stats Stats { get; private set; }

        private void Awake()
        {
            Stats.Init();
        }

        private void Update()
        {
            Move();
            Loop();
        }

        private void LateUpdate()
        {
            rotateCamera.Rotate(transform,Vector2.one);
        }

        private void Move()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            
            var dir = (rotateCamera.transform.forward * v + rotateCamera.transform.right * h).normalized;
            
            var yVel = body.velocity.y;
            body.velocity = new Vector3(dir.x * Stats.GetSpeed(), yVel, dir.z * Stats.GetSpeed());
        }

        private void Loop()
        {
            if (Physics.BoxCast(rotateCamera.transform.position, size, rotateCamera.transform.forward,
                    out var hit, rotateCamera.transform.rotation, rayDistance, layerMask))
            {
                if (hit.collider.TryGetComponent(out InteractableObject  interactableObject) && Input.GetMouseButton(1))
                    interactableObject.Execute();
            }
        }
        
        void OnDrawGizmos()
        {
            if (rotateCamera == null) return;

            var origin = rotateCamera.transform.position;
            var dir = rotateCamera.transform.forward;
            
            var end = origin + dir * rayDistance;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, end);
            
            Gizmos.matrix = Matrix4x4.TRS(end, rotateCamera.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, size * 2f);
        }
    }
}