using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BarChart : MonoBehaviour {

    private const int _chartAreaIntervalRatio = 10;

    private Color[] _color = { Color.black, Color.white, Color.yellow, Color.red, Color.magenta, Color.grey, Color.green, Color.cyan };
    [SerializeField]
    private Color[] _customizedColor;

    [SerializeField]
    private Transform _barHoldPrefab;
    [SerializeField]
    private Bar _barPrefab;
    [SerializeField]
    private LabelView _labelView;
    [SerializeField]
    private Label _labelPrefab;
    
    private List<BarManager> barManagers = new List<BarManager>();

    [SerializeField]
    private float _lineThickness = 1;
    private float _chartHeight;
    [SerializeField]
    private bool _isLineChart = true;
    [SerializeField]
    private bool _isBarChart;
    [SerializeField]
    private bool _isLabelActive = true;

    private int _longestList;
    private float _highestValue;

    public List<List<float>> tmpList;
    public List<string> nameTmpList;

    public bool IsLineChart {
        get { return _isLineChart; }
        set { _isLineChart = value; }
    }
    public bool IsLabelActive
    {
        get { return _isLabelActive; }
        set { _isLabelActive = value; }
    }
    public bool IsBarChart
    {
        get { return _isBarChart; }
        set { _isBarChart = value; }
    }
    
    private void Start()
    {
        IsLineChart = true;

        List<List<float>> variousBar = new List<List<float>>();

        List<int> bar1 = new List<int>(); 
        List<int> bar2 = new List<int>(); 
        List<int> bar3 = new List<int>();
        List<int> bar4 = new List<int>();


        List<float> fbar1 = new List<float>();
        List<float> fbar2 = new List<float>();
        List<float> fbar3 = new List<float>();
        List<float> fbar4 = new List<float>();

        fbar1.Add(1.5f);
        fbar1.Add(5.2f);
        fbar1.Add(50.3f);
        fbar2.Add(40.3f);
        fbar3.Add(60.1f);
        fbar3.Add(70);
        fbar2.Add(40);
        fbar2.Add(40);
        fbar3.Add(60);
        fbar3.Add(70);
        fbar4.Add(50);
        fbar4.Add(50);
        fbar4.Add(50);
        fbar4.Add(50);
        fbar4.Add(50.2f);
        fbar4.Add(50);
        fbar4.Add(50);
        fbar4.Add(50.3f);
        fbar4.Add(50);
        fbar4.Add(50.5f);

        variousBar.Add(fbar1);
        variousBar.Add(fbar2);
        variousBar.Add(fbar3);
        variousBar.Add(fbar4);

        List<string> names = new List<string>();

        names.Add("kk");
        names.Add("ww");
        names.Add("jj");
        names.Add("jj");
        names.Add("jj");
        names.Add("jj");

        tmpList = variousBar;
        nameTmpList = names;
        MakeChart(variousBar,names);
    }

    //List<List<int>> NullFunction(List<List<int>> list) { return list; }

    //public delegate List<List<int>> convertListDelegate<T>(List<T> list);

    //public void MakeChart<T>(List<T> list,convertListDelegate<T> listDelegate)
    //{
    //    List<List<int>> cvtList = listDelegate(list);
    //    MakePoints(cvtList);
    //    MakeLines(null);
    //}
    //public void MakeChart<T>(List<T> list, convertListDelegate<T> listDelegate, List<string> names)
    //{
    //    List<List<int>> cvtList = listDelegate(list);
    //    MakePoints(cvtList);
    //    MakeLines(names);
    //}

    public void MakeChart(List<List<int>> list)
    {
        MakePoints(list);
        MakeLines();
        MakeLabels(null);
    }
    public void MakeChart(List<List<int>> list, List<string> names)
    {
        MakePoints(list);
        MakeLines();
        MakeLabels(names);
    }
    public void MakeChart(List<List<float>> list)
    {
        MakePoints(list);
        MakeLines();
        MakeLabels(null);
    }
    public void MakeChart(List<List<float>> list, List<string> names)
    {
        Transform[] trans = GetComponentsInChildren<Transform>();

        MakePoints(list);
        MakeLines();
        MakeLabels(names);
    }
    
    public void UpdateLines(List<List<int>> list)
    {
        BackToInit();
        MakeChart(list);
    }
    public void UpdateLines(List<List<int>> list,List<string> names)
    {
        BackToInit();
        MakeChart(list,names);
    }
    public void UpdateLines(List<List<float>> list)
    {
        BackToInit();
        MakeChart(list);

    }
    public void UpdateLines(List<List<float>> list, List<string> names)
    {
        BackToInit();
        MakeChart(list, names);
    }

    private void BackToInit()
    {
        EraseAllChartChildren();
        EraseAllLabels();
        barManagers = new List<BarManager>();
    }
    
    public void ChangeLineState(Label label)
    {
        List<Bar> bars = label.barManager.bars;
        bool det = label.isBarActive;

        foreach (Bar obj in label.barManager.bars)
        {
            obj.gameObject.SetActive(!det);
        }

        label.isBarActive = !det;
    }
    
    private void MakePoints(List<List<int>> vals) {

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 rectSize = new Vector2(rectTransform.rect.width,rectTransform.rect.height);
        Vector2 rectSize02 = rectTransform.sizeDelta;
        _chartHeight = rectSize.y;

        _longestList = FindLongestListCount<int>(vals);
        _highestValue = FindMaxValueIn2DList(vals);

        for (int i = 0; i < _longestList/*가장 많이 실행한 훈련의 크기*/; i++) {
            //Make Bar & Set it as child of this object.
            Transform newBarHolder = Instantiate(_barHoldPrefab,transform);

            //이부분은 차트에서 한 column을 생성한다.
            for(int j = 0; j < vals.Count/*훈련의 갯수*/; j++)
            {
                if (vals[j].Count < i + 1) continue;
                
                //Make Bar & Set it as child of this object
                Bar newBar = Instantiate(_barPrefab, newBarHolder.transform);

                if (_isBarChart)
                {
                    newBar.bar.GetComponent<Image>().color = GetColor(j);
                }

                
                int value = vals[j][i];
                RectTransform rt = newBar.bar.rectTransform;
                float normalizedValue = ((float)value / (float)_highestValue) * 0.7f;
                //Setting value's text
                newBar.barValue.text = value.ToString();
                //Setting bar's height
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, _chartHeight * normalizedValue);

                //Save Bar information into BarHolder

                if (i == 0)
                {
                    BarManager barManager = new BarManager();
                    barManager.bars.Add(newBar);
                    barManager.index = j;
                    barManagers.Add(barManager);
                }
                else
                {
                    barManagers[j].bars.Add(newBar);
                }
            }
            //Save BarHolder information into List
            
            
            //일정비율 이상이면 그 간격으로 유지하기 위해 그 크기만큼 Chart area를 넓혀준다.
            if(i > _chartAreaIntervalRatio)
                ExpandChartArea(rectSize);
        }
    }
    private void MakePoints(List<List<float>> vals)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 rectSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        Vector2 rectSize02 = rectTransform.sizeDelta;
        _chartHeight = rectSize.y;

        _longestList = FindLongestListCount<float>(vals);
        _highestValue = FindMaxValueIn2DList(vals);

        for (int i = 0; i < _longestList/*가장 많이 실행한 훈련의 크기*/; i++)
        {
            //Make Bar & Set it as child of this object.
            Transform newBarHolder = Instantiate(_barHoldPrefab, transform);

            //이부분은 차트에서 한 column을 생성한다.
            for (int j = 0; j < vals.Count/*훈련의 갯수*/; j++)
            {
                if (vals[j].Count < i + 1) continue;

                //Make Bar & Set it as child of this object
                Bar newBar = Instantiate(_barPrefab, newBarHolder.transform);

                if (_isBarChart)
                {
                    newBar.bar.GetComponent<Image>().color = GetColor(j);
                }


                float value = vals[j][i];
                RectTransform rt = newBar.bar.rectTransform;
                float normalizedValue = ((float)value / (float)_highestValue) * 0.7f;
                //Setting value's text
                newBar.barValue.text = value.ToString();
                //Setting bar's height
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, _chartHeight * normalizedValue);

                //Save Bar information into BarHolder

                if (i == 0)
                {
                    BarManager barManager = new BarManager();
                    barManager.bars.Add(newBar);
                    barManager.index = j;
                    barManagers.Add(barManager);
                }
                else
                {
                    barManagers[j].bars.Add(newBar);
                }
            }
            //Save BarHolder information into List


            //일정비율 이상이면 그 간격으로 유지하기 위해 그 크기만큼 Chart area를 넓혀준다.
            if (i > _chartAreaIntervalRatio)
                ExpandChartArea(rectSize);
        }
    }
    
    //Make Lines.  
    //row's length is equal with the longest length of list.
    public void MakeLines()
    {

        for (int i = 0; i < barManagers.Count; i++)
        {
            for (int j = 1; j < barManagers[i].bars.Count ; j++)
            {
                try
                {
                    //현재 BarHolder에 있는 bar에서 이전 BarHolder에 있는 bar로 이어주는 선을 만듦.
                    MakeLine(barManagers[i].bars[j-1], barManagers[i].bars[j], i);

                }catch (System.ArgumentOutOfRangeException e)
                {
                    //해당 훈련의 리스트에 값이 없을 때 다음 훈련으로 넘어감.
                }
            }
        }
    }

    private void MakeLabels(List<string> names)
    {
        for(int i = 0; i < barManagers.Count; i++)
        {
            string LabelName;
            if (names == null)
            {
                LabelName = i.ToString();
            }
            else
            {
                LabelName = names[i];
            }

            MakeLabel(i, names[i]);
        }
    }

    //Make Line.
    private void MakeLine(Bar toBar, Bar fromBar,int colorIdx)
    {
        Vector2 toVector = toBar.circle.rectTransform.position;
        Vector2 fromVector = fromBar.circle.rectTransform.position;

        //임시. toVector의 X값이 바뀌지 않아서 임시로 각 circle의 위치의 차를 구해 그 값으로 X값을 대체.
        float newX = fromVector.x - GetComponent<RectTransform>().rect.width / _longestList;
        toVector = new Vector2(newX, toVector.y);
        Vector2 subVector = toVector - fromVector;

        Image lineImg = fromBar.line;
        RectTransform lineTransform = fromBar.line.rectTransform;

        lineImg.color = GetColor(colorIdx);
        lineTransform.sizeDelta = new Vector2(_lineThickness, subVector.magnitude);
        lineTransform.Rotate(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(subVector.y, subVector.x) - 90));
    }
    private void MakeLabel(int idx, string name)
    {
        Label label = Instantiate(_labelPrefab, _labelView.transform) as Label;
        label.img.color = GetColor(idx);
        label.label.text = name;
        label.barManager = barManagers[idx];
        label.isBarActive = true;
        label.gameObject.SetActive(true);
        _labelView.labels.Add(label);
    }

    private void EraseAllChartChildren()
    {
        Transform[] tfs = GetComponentsInChildren<Transform>();

        for(int i = 0; i < tfs.Length; i++)
        {
            if(tfs[i] != transform)
            {
                DestroyObject(tfs[i].gameObject);
            }
        }
    }
    private void EraseAllLabels()
    {
        foreach(Label label in _labelView.labels)
        {
            DestroyObject(label.gameObject);
        }

    }
    //Expand Chart Area as some ratio of it's width.
    private void ExpandChartArea(Vector2 rectSize)
    {
        GetComponent<RectTransform>().sizeDelta += new Vector2(rectSize.x / _chartAreaIntervalRatio, 0);
    }
    //Find longest list
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
    //Find Max number in 2-D list
    private float FindMaxValueIn2DList(List<List<float>> val)
    {
        float max = 0;

        for (int i = 0; i < val.Count; i++)
        {
            for (int j = 0; j < val[i].Count; j++)
            {
                float value = val[i][j];
                max = max > value ? max : value;
            }
        }

        return max;
    }
    private float FindMaxValueIn2DList(List<List<int>> val)
    {
        int max = 0;

        for (int i = 0; i < val.Count; i++)
        {
            for (int j = 0; j < val[i].Count; j++)
            {
                int value = val[i][j];
                max = max > value ? max : value;
            }
        }

        return max;
    }
    //Get Color by Index.
    private Color GetColor (int colorIdx)
    {
        try
        {
            if (_customizedColor.Length < colorIdx)
                return _color[colorIdx];
            else
                return _customizedColor[colorIdx];

        } catch(System.IndexOutOfRangeException e)
        {//when there's no color that can be applied.
            return _color[colorIdx];
        }

    }
}
