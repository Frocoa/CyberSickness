using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
//using UnityEngine.Windows;

public class CSVWriter
{

    private readonly StringBuilder stringBuilder = new("Time, Value");
    
    public void AddValueToString(float time, float value)
    {
        stringBuilder.Append('\n').Append(time.ToString()).Append(',').Append(value.ToString());
    }


    public void ExportStringToCSV()
    {
        string path = Application.streamingAssetsPath;
        string data = stringBuilder.ToString();

        if (!Directory.Exists(path)) 
            Directory.CreateDirectory(path);

        path = Path.Combine(path, "output.csv");
        File.WriteAllText(path, data);
    }
}
