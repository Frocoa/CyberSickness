using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace CustomLocomotion
{
    public class HookLocomotion : LocomotionProvider
    {

        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        
        [SerializeField]
        private InputActionProperty leftHandTriggerAction = new(new InputAction("Left Hand Trigger", expectedControlType: "Button"));
        
        [SerializeField]
        private InputActionProperty rightHandTriggerAction = new(new InputAction("Right Hand Trigger", expectedControlType: "Button"));

        [SerializeField] private float strength;


        private Rigidbody xrRigidBody;
        private bool attemptedGetRigidBody;
        private LayerMask wallLayer;
        
        private void Start()
        {
            leftHandTriggerAction.action.performed += ShootLeftHook;
            rightHandTriggerAction.action.performed += ShootRightHook;
            wallLayer = LayerMask.GetMask("Wall");
        }
        
        private void ShootLeftHook(InputAction.CallbackContext ctx)
        {
            FindRigidBody();
            ShootRayCast(leftHand);
        }
        
        private void ShootRightHook(InputAction.CallbackContext ctx)
        {
            FindRigidBody();
            ShootRayCast(rightHand);
        }

        private void ShootRayCast(Transform origin)
        {
            if (!Physics.Raycast(origin.position, origin.TransformDirection(Vector3.forward), out RaycastHit hit,
                    Mathf.Infinity, wallLayer)) return;

            xrRigidBody.velocity = new Vector3(0, xrRigidBody.velocity.y, 0);
            Vector3 direction = (hit.transform.position - xrRigidBody.position).normalized;
            if (direction.y > 0)
            {
                direction.y += 0.5f; // Little jump
            }

            xrRigidBody.AddForce(direction * strength);
        }
        
        private void FindRigidBody()
        {
            var xrOrigin = system.xrOrigin?.Origin;
            if (xrOrigin == null || xrRigidBody != null || attemptedGetRigidBody) return;
            if (!xrOrigin.TryGetComponent(out xrRigidBody) && xrOrigin != system.xrOrigin.gameObject)
                system.xrOrigin.TryGetComponent(out xrRigidBody);
            attemptedGetRigidBody = true;
        }
    }
}
