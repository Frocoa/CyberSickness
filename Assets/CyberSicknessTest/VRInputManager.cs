using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRInputManager : MonoBehaviour
{
    [SerializeField] SimulatorManager simulator;
    private List<InputDevice> myDevices;

    private bool lastButtonEvent = false;

    private void Awake()
    {
        myDevices = new List<InputDevice>();
    }

    void OnEnable()
    {
        var inputDevices = new List<InputDevice>();
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);

        foreach(InputDevice device in allDevices)
            InputDevices_deviceConnected(device);

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;


        var leftHandedControllers = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);

    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 _))
            myDevices.Add(device); // Add any devices that have a stick
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        if (myDevices.Contains(device))
            myDevices.Remove(device);
    }

    // Update is called once per frame
    private void Update()
    {
        bool tempState = false;

        foreach (var device in myDevices)
        {
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 stickState)) // did get a value
            {
                simulator.UpdateBar(stickState.x);
            }


            bool primaryButtonState = false;
            tempState = device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState) // did get a value
                        && primaryButtonState // the value we got
                        || tempState; // cumulative result from other controllers

        }
        if (tempState != lastButtonEvent)
        {
            lastButtonEvent = tempState;
            simulator.OnButtonPressed(tempState);
        }
    }
}
