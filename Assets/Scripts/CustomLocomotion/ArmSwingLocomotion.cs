using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace CustomLocomotion
{
    public class ArmSwingLocomotion : ContinuousMoveProviderBase
    {
        [SerializeField] private float breakingForce;
        [SerializeField] private float energyLoss;
        [SerializeField] private float strength;
        [SerializeField] private float maxEnergy;
        
        [SerializeField]
        private InputActionProperty leftHandPositionAction = new(new InputAction("Left Hand Position", expectedControlType: "Vector3"));
        
        [SerializeField]
        private InputActionProperty rightHandPositionAction = new(new InputAction("Right Hand Position", expectedControlType: "Vector3"));

        [SerializeField]
        private InputActionProperty breakAction = new(new InputAction("Break", expectedControlType: "Button"));

        private bool started;
        
        private float lastLeftHandYAxis;
        private float lastRightHandYAxis;

        private float deltaLeftHandYAxis;
        private float deltaRightHandYAxis;

        private float energy;
        private bool breaking;

        private float initialTime;
        
        private void Start()
        {
            initialTime = Time.time;
            
            leftHandPositionAction.action.performed += MoveLeftHand;
            rightHandPositionAction.action.performed += MoveRightHand;
            breakAction.action.performed += (context => breaking = true);
            breakAction.action.canceled += (context => breaking = false);

            lastLeftHandYAxis = leftHandPositionAction.action.ReadValue<Vector3>().y;
            lastRightHandYAxis = rightHandPositionAction.action.ReadValue<Vector3>().y;
            energy = 0;
        }
        
        private void MoveLeftHand(InputAction.CallbackContext ctx)
        {
            float position = ctx.ReadValue<Vector3>().y;
            energy += Mathf.Abs(position - lastLeftHandYAxis) * strength * Time.deltaTime;
            lastLeftHandYAxis = position;
        }
        
        private void MoveRightHand(InputAction.CallbackContext ctx)
        {
            float position = ctx.ReadValue<Vector3>().y;
            energy +=  Mathf.Abs(position - lastRightHandYAxis) * strength * Time.deltaTime;
            lastRightHandYAxis = position;
        }
        
        protected override Vector2 ReadInput()
        {
            if (!started && Time.time - initialTime > 1.0)
            {
                started = true;
            }

            if (!started) return Vector2.zero;
            
            if (breaking)
            {
                energy = Math.Max(energy - breakingForce, 0);
            }
            
            energy =  Math.Max(energy - energyLoss, 0);
            energy = Math.Min(energy, maxEnergy);
            return new Vector2(0,energy);
        }
    }
}
