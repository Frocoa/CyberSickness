using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices) {
            Debug.Log(string.Format("Device found with name '{0}' and characteristic '{1}'", device.name, device.characteristics.ToString()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
