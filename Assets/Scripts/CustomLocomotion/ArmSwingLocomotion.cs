using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace CustomLocomotion
{
    public class ArmSwingLocomotion : LocomotionProvider
    {

        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        
        [SerializeField]
        private InputActionProperty leftHandPositionAction = new InputActionProperty(new InputAction("Left Hand Position", expectedControlType: "Vector3"));
        
        [SerializeField]
        private InputActionProperty rightHandPositionAction = new InputActionProperty(new InputAction("Right Hand Position", expectedControlType: "Vector3"));


        private Rigidbody xrRigidBody;
        private bool attemptedGetRigidBody;
        
        private void Start()
        {
            leftHandPositionAction.action.performed += ShootLeftHook;
            rightHandPositionAction.action.performed += ShootRightHook;
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
            if (Physics.Raycast(origin.position, origin.TransformDirection(Vector3.forward), out RaycastHit hit,
                    Mathf.Infinity, 1))
            {
                Vector3 direction = (hit.transform.position - xrRigidBody.position);
                if (direction.y > 0)
                {
                    direction.y *= 5.0f;
                }
                xrRigidBody.AddForce(direction);
            }
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
