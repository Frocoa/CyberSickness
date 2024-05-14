using System;
using System.Collections;
using System.Collections.Generic;
using CustomLocomotion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionLoader : MonoBehaviour
{
    private void Start()
    {
        switch (StateManager.Instance.SelectedLocomotion)
        {
            case StateManager.LocomotionTechnique.Walk:
                GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
                gameObject.AddComponent<CharacterController>();
                break;
            case StateManager.LocomotionTechnique.Swing:
                GetComponent<ArmSwingLocomotion>().enabled = true;
                break;
            case StateManager.LocomotionTechnique.Hook:
                GetComponent<HookLocomotion>().enabled = true;
                break;
            case StateManager.LocomotionTechnique.Propel:
                GetComponent<ActionBasedPropelLocomotionProvider>().enabled = true;
                break;
        }

        
    }
}
