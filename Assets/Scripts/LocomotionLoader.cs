using System;
using System.Collections;
using System.Collections.Generic;
using CustomLocomotion;
using Emotiv;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using EmotivUnityPlugin;

public class LocomotionLoader : MonoBehaviour
{

    [SerializeField] private GameObject barManager;
    private void Start()
    {
        switch (StateManager.Instance.SelectedLocomotion)
        {
            case StateManager.LocomotionTechnique.Walk:
                GetComponent<CustomContinuousMoveProvider>().enabled = true;
                gameObject.AddComponent<CharacterController>();
                break;
            case StateManager.LocomotionTechnique.Swing:
                GetComponent<ArmSwingLocomotion>().enabled = true;
                gameObject.AddComponent<CharacterController>();
                break;
            case StateManager.LocomotionTechnique.Hook:
                GetComponent<HookLocomotion>().enabled = true;
                GetComponent<CapsuleCollider>().enabled = true;
                break;
            case StateManager.LocomotionTechnique.Propel:
                GetComponent<ActionBasedPropelLocomotionProvider>().enabled = true;
                break;
        }
        
        EmotivManager.Instance.SetBar(barManager);
        EmotivManager.Instance.StartExperiment();
    }
}
