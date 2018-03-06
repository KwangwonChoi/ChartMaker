using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BarChart : MonoBehaviour {

    public Bar barPrefab;
    public int[] inputValues;
    public string[] labels;

    List<Bar> bars = new List<Bar>();


    private float _chartHeight;

    private void Start()
    {
        _chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        
        DisplayGraph(inputValues);
        
    }

    void DisplayGraph(int[] vals) {
        int maxValue = vals.Max();

        for (int i = 0; i < vals.Length; i++) {
            Bar newBar = Instantiate(barPrefab) as Bar;
            newBar.transform.SetParent(transform);
            RectTransform rt = newBar.bar.GetComponent<RectTransform>();
            float normalizedValue = ((float)vals[i] / (float)maxValue) * 0.95f;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, _chartHeight * normalizedValue);
        }
    }
}
