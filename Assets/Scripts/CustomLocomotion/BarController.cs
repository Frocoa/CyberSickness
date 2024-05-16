using System;
using Emotiv;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CustomLocomotion
{
    public class BarController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The Input System Action that will be used to read Move data from the left hand controller. Must be a Value Vector2 Control.")]
        InputActionProperty leftHandMoveAction = new(new InputAction("Left Hand Move", expectedControlType: "Vector2"));
        
        

        private void Start()
        {
            leftHandMoveAction.action.performed += context =>  EmotivManager.Instance.UpdateBar(context.ReadValue<Vector2>().x);
        }
    }
}
