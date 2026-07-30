using Game.ObjectInteractable;
using Game.Unit.Component;
using Game.Abstraction;
using Game.Component;
using UnityEngine;
using System;

namespace Game.Unit
{
    public class Player : MonoBehaviour
    {
        public event Action OnItemsEditInHandEv;
        
        [field: SerializeField] public LHandInventory LHand { get; private set; }
        
        [SerializeField] private Rigidbody body;
        [SerializeField] private RotateCamera  rotateCamera;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float rayDistance;
        [SerializeField] private Vector3 size;
        [SerializeField] private Color colorGizmos;
        [SerializeField] private Color colorPickup;

        [field: SerializeField] public Stats Stats { get; private set; }
        
        private Collider[] _colliders = new Collider[10];

        private void Awake()
        {
            Stats.Init();
        }

        public void Dead()
        {
            body.constraints = RigidbodyConstraints.None;
        }

        public void ItemsEditInHand()
        {
            OnItemsEditInHandEv?.Invoke();
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
            if (Physics.Raycast(rotateCamera.transform.position,rotateCamera.transform.forward,out var hit, rayDistance, layerMask))
            {
                if (hit.collider.TryGetComponent(out InteractableObject interactableObject) && Input.GetMouseButtonDown(0))
                    interactableObject.Execute();
            }
            
            Physics.OverlapSphereNonAlloc(transform.position, rayDistance, _colliders, layerMask);

            if (_colliders != null && _colliders.Length > 0 && Input.GetKeyDown(KeyCode.E))
            {
                foreach (var collider in _colliders)
                {
                    if (collider == null) break;
                    if (collider.TryGetComponent(out ItemRes res))
                        res.Execute();
                }
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
            
            Gizmos.color = colorPickup;
            Gizmos.DrawWireSphere(transform.position, rayDistance);
        }
        
    }
}