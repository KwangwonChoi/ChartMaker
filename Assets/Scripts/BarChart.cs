using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BarChart : MonoBehaviour {

    [SerializeField]
    private BarHolder _barHoldPrefab;
    [SerializeField]
    private Bar _barPrefab;
    [SerializeField]
    private int[] _inputValues;

    private List<List<int>> variousBar = new List<List<int>>();
    List<int> bar1;
    List<int> bar2;
    private List<List<Bar>> bars = new List<List<Bar>>();

    [SerializeField]
    private float _lineThickness;
    private float _chartHeight;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private bool _isLineChart;
    [SerializeField]
    private bool _isBarChart;
    
    private void Start()
    {
        _chartHeight = GetComponent<RectTransform>().sizeDelta.y;

        variousBar = new List<List<int>>();
        bar1 = new List<int>();
        bar2 = new List<int>();

        bar1.Add(1);
        bar1.Add(4);
        
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        bar2.Add(5);
        List<int> bar3 = new List<int>();
        bar3.Add(3);
        bar3.Add(3);
        bar3.Add(3);
        bar3.Add(3);

        variousBar.Add(bar1);
        variousBar.Add(bar2);
        variousBar.Add(bar3);

        MakePoints(variousBar);
        if (_isLineChart)
            MakeLines();
    }


    
    private void MakePoints(List<List<int>> vals) {
        int maxValue = 5;
        
        Vector2 rectSize = GetComponent<RectTransform>().sizeDelta;
        int maxCount = FindLongestListCount<int>(vals);

        for (int i = 0; i < maxCount/*가장 많이한 훈련의 크기*/; i++) {
            //Make Bar & Set it as child of this object.
            BarHolder newBarHolder = Instantiate(_barHoldPrefab,transform) as BarHolder;

            //이부분은 차트에서 한 column을 생성한다.
            for(int j = 0; j < vals.Count/*훈련의 갯수*/; j++)
            {
                if (vals[j].Count < i + 1) continue;
                
                //Make Bar & Set it as child of this object.
                Bar newBar = Instantiate(_barPrefab, newBarHolder.transform);

                if (_isBarChart)
                {
                    newBar.bar.GetComponent<Image>().color = _color;
                }

                //Setting bar's height.
                RectTransform rt = newBar.bar.rectTransform;
                float normalizedValue = ((float)vals[j][i] / (float)maxValue) * 0.7f;
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, _chartHeight * normalizedValue);

                //Save Bar information into List
                if (bars.Count < vals.Count)
                {
                    List<Bar> bar = new List<Bar>();
                    bar.Add(newBar);
                    bars.Add(bar);
                }

                bars[j].Add(newBar);
            }
            
            if(i > 10)
                ExpandChartArea(rectSize);
        }
    }

    //fint longest list
    private int FindLongestListCount<T>(List<List<T>> list)
    {
        int ret = 0;

        for (int i = 0; i < list.Count; i++)
        {
            int tmp = list[i].Count;
            ret = ret > tmp ? ret : tmp;
        }

        return ret;
    }

    private void MakeLines()
    {
        int maxCount = FindLongestListCount<int>(variousBar);

        for (int i = 1; i < maxCount; i++)
        {
            for (int j = 0; j < bars.Count; j++)
            {
                try
                {
                    MakeLine(bars[j][i - 1], bars[j][i]);
                }catch(System.ArgumentOutOfRangeException e) { }
            }
        }
    }

    private void ExpandChartArea(Vector2 rectSize) {
        GetComponent<RectTransform>().sizeDelta += new Vector2(rectSize.x / 10, 0);
    }

    private void MakeLine(Bar toBar, Bar fromBar)
    {
        Vector3 toVector = toBar.transform.position;
        Vector3 fromVector = fromBar.transform.position;

        //임시. toVector의 X값이 바뀌지 않아서 임시로 각 circle의 위치의 차를 구해 그 값으로 X값을 대체.
        float newX = fromVector.x - GetComponent<RectTransform>().sizeDelta.x / _inputValues.Length;
        toVector = new Vector2(newX, toVector.y);
        Vector3 subVector = toVector - fromVector;

        Image lineImg = fromBar.line;
        RectTransform lineTransform = fromBar.line.rectTransform;

        lineImg.color = _color;
        lineTransform.sizeDelta = new Vector3(_lineThickness, subVector.magnitude);
        lineTransform.Rotate(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(subVector.y, subVector.x) - 90));
    }
}
