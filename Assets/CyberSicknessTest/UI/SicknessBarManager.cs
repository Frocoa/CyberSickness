using UnityEngine;
using UnityEngine.UI;

public class SicknessBarManager : MonoBehaviour
{
    [SerializeField]
    private Slider sicknessBar;

    [SerializeField]
    private float sensibility = 0.1f;

    private float sicknessValue = 0;

    public float SicknessValue { 
        get => sicknessValue; 
        private set { 
            sicknessValue = value; sicknessBar.value = value; 
        }
    }

   
    public void AddValueToMeter(float value)
    {
        SicknessValue += value * sensibility;
    }
}
