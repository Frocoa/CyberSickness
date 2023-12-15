using UnityEngine;
using UnityEngine.Splines;

public class SimulatorManager : MonoBehaviour
{

    [SerializeField] private GameObject barManager;
    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private float splineSpeed = 20.0f;
    private SicknessBarManager barManagerScript;

    private bool started = false;
    private float simulationTime;
    private readonly CSVWriter writer = new();

    private void Awake()
    {
        barManagerScript = barManager.GetComponent<SicknessBarManager>();
    }

    public void UpdateBar(float value)
    {
        barManagerScript.AddValueToMeter(value);
    }

    public void OnButtonPressed(bool pressed)
    {
        if (!pressed) return;
        Debug.Log("Button Pressed");
        if (started)
        {
            EndSimulation();
        }
        else
        {
            StartSimulation();
            
        }
    }
   public void StartSimulation()
    {
        started = true;
        Debug.Log("Started...");
        splineAnimate.MaxSpeed = splineSpeed;
        splineAnimate.Play();
    }

    public void EndSimulation()
    {
        Debug.Log("Saving CSV...");
        writer.ExportStringToCSV();
        splineAnimate.Pause();
    }

    private void Update()
    {
        // if (!started) return;

        simulationTime += Time.deltaTime;
        writer.AddValueToString(simulationTime, barManagerScript.SicknessValue);
    }
}
