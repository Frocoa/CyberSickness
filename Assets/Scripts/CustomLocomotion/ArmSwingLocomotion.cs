using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace CustomLocomotion
{
    public class ArmSwingLocomotion : ContinuousMoveProviderBase
    {
        
        [SerializeField]
        private InputActionProperty leftHandPositionAction = new InputActionProperty(new InputAction("Left Hand Position", expectedControlType: "Vector3"));
        
        [SerializeField]
        private InputActionProperty rightHandPositionAction = new InputActionProperty(new InputAction("Right Hand Position", expectedControlType: "Vector3"));
        
        private float lastLeftHandYAxis;
        private float lastRightHandYAxis;

        private float deltaLeftHandYAxis;
        private float deltaRightHandYAxis;
        
        private void Start()
        {
            leftHandPositionAction.action.performed += MoveLeftHand;
            rightHandPositionAction.action.performed += MoveRightHand;

            lastLeftHandYAxis = leftHandPositionAction.action.ReadValue<Vector3>().y;
            lastRightHandYAxis = rightHandPositionAction.action.ReadValue<Vector3>().y;
        }
        
        private void MoveLeftHand(InputAction.CallbackContext ctx)
        {
            float position = ctx.ReadValue<Vector3>().y;
            deltaLeftHandYAxis =  position - lastLeftHandYAxis;
            lastLeftHandYAxis = position;
            print(position);
        }
        
        private void MoveRightHand(InputAction.CallbackContext ctx)
        {
            float position = ctx.ReadValue<Vector3>().y;
            deltaRightHandYAxis =  position - lastRightHandYAxis;
            lastRightHandYAxis = position;
        }
        
        protected override Vector2 ReadInput()
        {
            return new Vector2(0,Mathf.Abs(deltaLeftHandYAxis) + Mathf.Abs(deltaRightHandYAxis));
        }
    }
}
