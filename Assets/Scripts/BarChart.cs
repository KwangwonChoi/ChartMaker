using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BarChart : MonoBehaviour {

    public Bar barPrefab;

    public int[] inputValues;

    List<Bar> bars = new List<Bar>();

    [SerializeField]
    private float _lineThickness;
    private float _chartHeight;

    [SerializeField]
    private bool _isLineChart;
    [SerializeField]
    private bool _isBarChart;
    
    private void Start()
    {
        _chartHeight = GetComponent<RectTransform>().sizeDelta.y;
        
        DisplayGraph(inputValues);
    }

    void DisplayGraph(int[] vals) {
        int maxValue = vals.Max();

        ExpandChartArea(new Vector2(100, 100));

        Vector2 rectSize = GetComponent<RectTransform>().sizeDelta;

        for (int i = 0; i < vals.Length; i++) {
            //Make Bar & Set it as child of this object.
            Bar newBar = Instantiate(barPrefab) as Bar;
            newBar.transform.SetParent(transform);
            if (_isBarChart) {
                Color clr;
                clr = newBar.bar.GetComponent<Image>().color;
                clr.a = 1;
                newBar.bar.GetComponent<Image>().color = clr;
                    }

            //Setting bar's height.
            RectTransform rt = newBar.bar.rectTransform;
            float normalizedValue = ((float)vals[i] / (float)maxValue) * 0.7f;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, _chartHeight * normalizedValue);

            bars.Add(newBar);
            
            if(i > 10)
                ExpandChartArea(rectSize);
        }
        //ExpandChartArea(new Vector2(-100, -100));
        if(_isLineChart)
            MakeLines();
    }

    private void MakeLines() {

        for(int i = 1; i < bars.Count; i++)
        {
            MakeLine(bars[i-1].circle.transform.position, bars[i].circle.transform.position, i);
        }

    }

    private void ExpandChartArea(Vector2 rectSize) {
        GetComponent<RectTransform>().sizeDelta += new Vector2(rectSize.x / 10, 0);
    }

    private void MakeLine(Vector3 toVector, Vector3 fromVector,int idx) {
        Vector3 subVector = toVector - fromVector;

        RectTransform line = bars[idx].line.rectTransform;
        
        line.sizeDelta = new Vector3(_lineThickness, subVector.magnitude);
        line.Rotate(new Vector3(0, 0, Mathf.Rad2Deg*Mathf.Atan2(subVector.y,subVector.x) - 90));
    }
        

    
}
