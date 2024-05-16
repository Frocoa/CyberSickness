using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace CustomLocomotion
{
    public class CustomContinuousMoveProvider : ActionBasedContinuousMoveProvider
    {
        [SerializeField] 
        private InputActionProperty rightHandRun = new(new InputAction("Right Hand Run", expectedControlType: "Button"));

        private void Start()
        {
            rightHandRun.action.performed += (context => moveSpeed *= 2);
            rightHandRun.action.canceled += (context => moveSpeed /= 2);
        }
    }
}
