using System;
using System.Collections.Generic;
using EmotivUnityPlugin;
using UnityEngine;
using System.IO;

namespace Emotiv
{
    public class EmotivManager : MonoBehaviour
    {
        public static EmotivManager Instance { get; private set; }
        
        private readonly EmotivUnityItf emotivItf = EmotivUnityItf.Instance;

        private const string ClientId = "0UQKT5NxAan9hitzZQCuMzww9eAuofYwjmoNiPKS";

        private const string ClientSecret =
            "sKOuFqZXuli8CtARWZjDHxK5bpLPWU7n7UMFbIqJARqSQs8inASpAligUJBJdosAxReT5sHqutoVrqGF6tNUXLtWsiCJwVMhCCEhYiFl9wZyrHjGxhfaDhG8GV7wh5wY";

        private const string AppName = "UnityApp";
        private const string AppVersion = "3.3.0";
        private const bool IsDataBufferUsing = false; // default subscribed data will not saved to Data buffer
        private bool alreadySubscribed;
        private bool alreadyAuthorized;
        private bool scanned;
        private bool started;

        private readonly string path = Application.dataPath + "/bar.txt";
        private readonly string powPath = Application.dataPath + "/power.txt";

        private SicknessBarManager barManagerScript;
        private float simulationTime;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void SetBar(GameObject barManager)
        {
            barManagerScript = barManager.GetComponent<SicknessBarManager>();
        }
        
        public void StartExperiment()
        {
            started = true;
            //List<string> streamNames = new List<string> { DataStreamName.BandPower };
            //emotivItf.SubscribeData(streamNames);
        }
        
        public void UpdateBar(float value)
        {
            barManagerScript.AddValueToMeter(value); 
        } 

        private void Start()
        {
            // init EmotivUnityItf without data buffer using
            emotivItf.Init(ClientId, ClientSecret, AppName, AppVersion, IsDataBufferUsing);

            // Start
            emotivItf.Start();
            List<string> streamNames = new List<string> { DataStreamName.BandPower };
            emotivItf.UnSubscribeData(streamNames);
            

            if (!File.Exists(path))
            {
                File.WriteAllText(path, "Login de barra\n");
            }
        }

        private void Update()
        {
            simulationTime += Time.deltaTime;
            
            if (emotivItf.IsAuthorizedOK && !scanned && !DataStreamManager.Instance.IsHeadsetScanning)
            {
                DataStreamManager.Instance.ScanHeadsets();
                scanned = true;
            }

            if (emotivItf.IsAuthorizedOK && scanned && !alreadyAuthorized && !DataStreamManager.Instance.IsHeadsetScanning)
            {
                //emotivItf.CreateSessionWithHeadset("EPOCPLUS-4A2C12F5");
                alreadyAuthorized = true;
            }
            //if (emotivItf.IsSessionCreated && !alreadySubscribed)
            //{
            //   
            //    emotivItf.UnSubscribeData(streamNames);
            //    alreadySubscribed = true;
            //}

            if (started)
            {
                if (!File.Exists(path))
                {
                    Debug.Log("El archivo no existe de algun modo");
                    return;
                }

                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine("Tiempo: " + simulationTime + " Valor: " +
                             barManagerScript.SicknessValue);
            }   
        }

        public void WritePowerMeasure(string data)
        {
            if (!File.Exists(powPath))
            {
                File.WriteAllText(powPath, "Login de power\n");
                return;
            }

            using StreamWriter sw = File.AppendText(powPath);
            sw.WriteLine("Tiempo: " + simulationTime + " Valores: " + data);
        }
    }
}
