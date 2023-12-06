using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private void OnEnable() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        ProgressBar sicknessBar = root.Q<ProgressBar>("SicknessBar");
        sicknessBar.value = 100;
    }
}
