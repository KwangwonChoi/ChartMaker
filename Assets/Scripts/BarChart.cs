using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BarChart : MonoBehaviour {

    private const int _chartAreaIntervalRatio = 10;
    private const float _chartHeightRatio = 0.95f;

    public int _makeType;
    private List<object> _paramData;

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
    [SerializeField]
    private Image _backLine;
    [SerializeField]
    private BackLineLabel _backLineText;
    [SerializeField]
    private Transform _backLineParent;
    [SerializeField]
    private Transform _backLineLabelParent;
    [SerializeField]
    private Transform _barGroup;

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

    [HideInInspector]
    public List<List<float>> tmpList;
    [HideInInspector]
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

    public delegate List<R> ExtractDataListFromList<T, R>(T list);
    public delegate List<string> ExtractNameFromList<T>(List<T> list);
    public delegate int ExtractIntegerFromData<T>(T data);
    public delegate float ExtractFloatFromData<T>(T data);

    public void MakeChart(List<List<int>> list)
    {
        _makeType = 1;
        MakePoints(list);
        MakeLines();
        MakeLabels(null);
        MakeChartBackLine();

        _paramData = new List<object>();
        _paramData.Add(list);
    }
    public void MakeChart(List<List<int>> list, List<string> names)
    {
        _makeType = 2;
        MakePoints(list);
        MakeLines();
        MakeLabels(names);
        MakeChartBackLine();

        _paramData = new List<object>();
        _paramData.Add(list);
        _paramData.Add(names);
    }
    public void MakeChart(List<List<float>> list)
    {
        _makeType = 3;
        MakePoints(list);
        MakeLines();
        MakeLabels(null);
        MakeChartBackLine();

        _paramData = new List<object>();
        _paramData.Add(list);
    }
    public void MakeChart(List<List<float>> list, List<string> names)
    {
        _makeType = 4;
        MakePoints(list);
        MakeLines();
        MakeLabels(names);
        MakeChartBackLine();

        _paramData = new List<object>();
        _paramData.Add(list);
        _paramData.Add(names);
    }
    public void MakeChart<T,R>(List<T> list, ExtractDataListFromList<T, R> extractDataDelegate, ExtractIntegerFromData<R> extractIntDelegate, ExtractNameFromList<T> extractNamesDelegate)
    {
        _makeType = 5;
        MakePoints<T, R>(list, extractDataDelegate, extractIntDelegate);
        MakeLines();
        MakeLabels(extractNamesDelegate(list));
        MakeChartBackLine();

        _paramData = new List<object>();
        _paramData.Add(list);
        _paramData.Add(extractDataDelegate);
        _paramData.Add(extractIntDelegate);
        _paramData.Add(extractNamesDelegate);
    }
    public void MakeChart<T,R>(List<T> list, ExtractDataListFromList<T, R> extractDataDelegate, ExtractFloatFromData<R> extractFloatDelegate, ExtractNameFromList<T> extractNamesDelegate)
    {
        _makeType = 6;
        MakePoints<T, R>(list, extractDataDelegate, extractFloatDelegate);
        MakeLines();
        MakeLabels(extractNamesDelegate(list));
        MakeChartBackLine();

        _paramData = new List<object>();
        _paramData.Add(list);
        _paramData.Add(extractDataDelegate);
        _paramData.Add(extractFloatDelegate);
        _paramData.Add(extractNamesDelegate);
    }
    
    public void UpdateLines(List<List<int>> list)
    {
        BackToInit();
        MakeChart(list);
    }
    public void UpdateLines(List<List<int>> list, List<string> names)
    {
        BackToInit();
        MakeChart(list, names);
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
    public void UpdateLines<T, R>(List<T> list, ExtractDataListFromList<T, R> extractDataDelegate, ExtractIntegerFromData<R> extractIntDelegate, ExtractNameFromList<T> extractNamesDelegate)
    {
        BackToInit();
        MakeChart(list, extractDataDelegate, extractIntDelegate, extractNamesDelegate);
    }
    public void UpdateLines<T, R>(List<T> list, ExtractDataListFromList<T, R> extractDataDelegate, ExtractFloatFromData<R> extractfloatDelegate, ExtractNameFromList<T> extractNamesDelegate)
    {
        BackToInit();
        MakeChart(list, extractDataDelegate, extractfloatDelegate, extractNamesDelegate);
    }

    public void Refresh<T,R>()
    {
        switch (this._makeType)
        {
            case 5:
                UpdateLines((List<T>)_paramData[0],
                    (ExtractDataListFromList<T, R>)_paramData[1],
                    (ExtractIntegerFromData<R>)_paramData[2],
                    (ExtractNameFromList<T>)_paramData[3]);
                break;
            case 6:
                UpdateLines((List<T>)_paramData[0],
                    (ExtractDataListFromList<T, R>)_paramData[1],
                    (ExtractFloatFromData<R>)_paramData[2],
                    (ExtractNameFromList<T>)_paramData[3]);
                break;
            default:
                Refresh();
                break;
        }
    }
    public void Refresh()
    {
        switch (this._makeType)
        {
            case 1: UpdateLines((List<List<int>>)_paramData[0]); break;
            case 2: UpdateLines((List<List<int>>)_paramData[0], (List<string>)_paramData[1]); break;
            case 3: UpdateLines((List<List<float>>)_paramData[0]); break;
            case 4: UpdateLines((List<List<float>>)_paramData[0], (List<string>)_paramData[1]); break;
            default:break;
        }
    }
    
    //Set certain type of ChartLine able or disable.
    public void ChangeLineVisibleState(Label label)
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
        Vector2 rectSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        Vector2 rectSize02 = rectTransform.sizeDelta;
        _chartHeight = rectSize.y;

        _longestList = FindLongestListCount<int>(vals);
        _highestValue = FindMaxValueIn2DList(vals);

        for (int i = 0; i < _longestList/*가장 많이 실행한 훈련의 크기*/; i++) {
            //Make Bar & Set it as child of this object.
            Transform newBarHolder = Instantiate(_barHoldPrefab, transform);

            //이부분은 차트에서 한 column을 생성한다.
            for (int j = 0; j < vals.Count/*훈련의 갯수*/; j++)
            {
                if (vals[j].Count < i + 1) continue;

                //Make Bar & Set it as child of this object
                Bar newBar = Instantiate(_barPrefab, newBarHolder.transform);
                // newBar.data = data[j]; //여기에 data를 넣어 bar에서 볼 수 있게 한다.

                if (_isBarChart)
                {
                    Image img = newBar.bar.GetComponent<Image>();
                    img.enabled = true;
                    img.color = GetColor(j);
                }


                int value = vals[j][i];
                RectTransform rt = newBar.bar.rectTransform;
                float normalizedValue = ((float)value / (float)_highestValue) * _chartHeightRatio;
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
            Transform newBarHolder = Instantiate(_barHoldPrefab, _barGroup);
            newBarHolder.gameObject.SetActive(true);
            //이부분은 차트에서 한 column을 생성한다.
            for (int j = 0; j < vals.Count/*훈련의 갯수*/; j++)
            {
                if (vals[j].Count < i + 1) continue;

                //Make Bar & Set it as child of this object
                Bar newBar = Instantiate(_barPrefab, newBarHolder.transform);
                newBar.gameObject.SetActive(true);
                if (_isBarChart)
                {
                    newBar.bar.GetComponent<Image>().color = GetColor(j);
                }


                float value = vals[j][i];
                RectTransform rt = newBar.bar.rectTransform;
                float normalizedValue = ((float)value / (float)_highestValue) * _chartHeightRatio;
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
    private void MakePoints<T, R>(List<T> vals, ExtractDataListFromList<T, R> extractDataDelegate, ExtractIntegerFromData<R> extractIntDelegate)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 rectSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        Vector2 rectSize02 = rectTransform.sizeDelta;
        _chartHeight = rectSize.y;

        _longestList = FindLongestListCount<T,R>(vals,extractDataDelegate);
        _highestValue = FindMaxValueIn2DList(vals, extractDataDelegate, extractIntDelegate);

        for (int i = 0; i < _longestList/*가장 많이 실행한 훈련의 크기*/; i++)
        {
            //Make Bar & Set it as child of this object.
            Transform newBarHolder = Instantiate(_barHoldPrefab, _barGroup);
            newBarHolder.gameObject.SetActive(true);
            //이부분은 차트에서 한 column을 생성한다.
            for (int j = 0; j < vals.Count/*훈련의 갯수*/; j++)
            {
                if (extractDataDelegate(vals[j]).Count < i + 1) continue;

                //Make Bar & Set it as child of this object
                Bar newBar = Instantiate(_barPrefab, newBarHolder.transform);
                newBar.gameObject.SetActive(true);

                R data = extractDataDelegate(vals[j])[i];

                newBar.data = data;

                if (_isBarChart)
                {
                    newBar.bar.GetComponent<Image>().color = GetColor(j);
                }
                
                int value = extractIntDelegate(data);
                RectTransform rt = newBar.bar.rectTransform;
                float normalizedValue = ((float)value / (float)_highestValue) * _chartHeightRatio;
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
    private void MakePoints<T, R>(List<T> vals, ExtractDataListFromList<T, R> extractDataDelegate, ExtractFloatFromData<R> extractfloatDelegate)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 rectSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        Vector2 rectSize02 = rectTransform.sizeDelta;
        _chartHeight = rectSize.y;

        _longestList = FindLongestListCount<T, R>(vals, extractDataDelegate);
        _highestValue = FindMaxValueIn2DList(vals, extractDataDelegate, extractfloatDelegate);

        for (int i = 0; i < _longestList/*가장 많이 실행한 훈련의 크기*/; i++)
        {
            //Make Bar & Set it as child of this object.
            Transform newBarHolder = Instantiate(_barHoldPrefab, _barGroup);
            newBarHolder.gameObject.SetActive(true);
            //이부분은 차트에서 한 column을 생성한다.
            for (int j = 0; j < vals.Count/*훈련의 갯수*/; j++)
            {
                if (extractDataDelegate(vals[j]).Count < i + 1) continue;

                //Make Bar & Set it as child of this object
                Bar newBar = Instantiate(_barPrefab, newBarHolder.transform);
                newBar.gameObject.SetActive(true);

                R data = extractDataDelegate(vals[j])[i];

                newBar.data = data;

                if (_isBarChart)
                {
                    newBar.bar.GetComponent<Image>().color = GetColor(j);
                }

                float value = extractfloatDelegate(data);
                RectTransform rt = newBar.bar.rectTransform;
                float normalizedValue = ((float)value / (float)_highestValue) * _chartHeightRatio;
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

    private void BackToInit()
    {
        //Revert prefabs
        RevertRectSize();
        EraseChartBackLine();
        EraseAllChartChildren();
        EraseAllLabels();

        //initialize Data
        _labelView.labels = new List<Label>();
        barManagers = new List<BarManager>();
    }
    private void MakeChartBackLine()
    {
        float unitValue = UnitValueMaker(_highestValue);
        int unitNum = (int)(_highestValue / unitValue);

        for (int i = 0; i < unitNum + 1; i++)
        {
            Image lineImg = Instantiate(_backLine, _backLineParent.transform) as Image;
            BackLineLabel lineLabel = Instantiate(_backLineText, _backLineLabelParent.transform) as BackLineLabel;
            lineLabel.gameObject.SetActive(true);
            float normalizedValue = ((float)unitValue / _highestValue) * _chartHeightRatio;

            Vector3 baseLinePosition = new Vector3(_backLineParent.transform.position.x,
                                                    _backLineParent.transform.position.y - _backLineParent.GetComponent<RectTransform>().rect.height / 2,
                                                    _backLineParent.transform.position.z);

            lineImg.rectTransform.position = baseLinePosition + new Vector3(0, i * normalizedValue * _chartHeight, 0);
            lineLabel.GetComponent<Text>().text = (unitValue * i).ToString();
            lineLabel.LineImage = lineImg;
        }
    }

    //Make Lines.  
    //row's length is equal with the longest length of list.
    private void MakeLines()
    {

        for (int i = 0; i < barManagers.Count; i++)
        {
            for (int j = barManagers[i].bars.Count; j > 0; j--)
            {
                try
                {
                    //현재 BarHolder에 있는 bar에서 이전 BarHolder에 있는 bar로 이어주는 선을 만듦.
                    MakeLine(barManagers[i].bars[j - 1], barManagers[i].bars[j], i);

                }
                catch (System.ArgumentOutOfRangeException e)
                {
                    //해당 훈련의 리스트에 값이 없을 때 다음 훈련으로 넘어감.
                }
            }
        }
    }
    private void MakeLine(Bar toBar, Bar fromBar, int colorIdx)
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

    private void MakeLabels(List<string> names)
    {
        for (int i = 0; i < barManagers.Count; i++)
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

    private float UnitValueMaker(float value)
    {
        int i = 0;

        if (value > 1)
        { for (; value / System.Math.Pow(10, i) > 10; i++) ; }
        else
        { for (; value / System.Math.Pow(10, i) < 1; i--) ; }

        if (value / System.Math.Pow(10, i) > 5)
        {
            return (float)System.Math.Pow(10, i);
        }
        else
        {
            return (float)System.Math.Pow(10, i - 1) * 5;
        }

    }

    private void EraseChartBackLine()
    {
        Transform[] tfs = _backLineParent.GetComponentsInChildren<Transform>();
        Transform[] labelTfs = _backLineLabelParent.GetComponentsInChildren<Transform>();

        EraseTransformChildren(tfs, _backLineParent.gameObject);
        EraseTransformChildren(labelTfs, _backLineLabelParent.gameObject);
    }
    private void EraseAllChartChildren()
    {
        Transform[] tfs = _barGroup.GetComponentsInChildren<Transform>();

        EraseTransformChildren(tfs, _barGroup.gameObject);
    }
    private void EraseTransformChildren(Transform[] tfs, GameObject obj)
    {
        for (int i = 0; i < tfs.Length; i++)
        {
            if (tfs[i] != obj.transform)
            {
                DestroyObject(tfs[i].gameObject);
            }
        }
    }
    private void EraseAllLabels()
    {
        foreach (Label label in _labelView.labels)
        {
            DestroyObject(label.gameObject);
        }

    }
    private void RevertRectSize()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Rect parentRect = GetComponentInParent<RectTransform>().rect;
        rectTransform.rect.Set(parentRect.x, parentRect.y, parentRect.width, parentRect.height);
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
    private int FindLongestListCount<T, R>(List<T> list, ExtractDataListFromList<T, R> extractDataDelegate){
        int ret = 0;

        for(int i = 0; i < list.Count; i++)
        {
            int tmp = extractDataDelegate(list[i]).Count;
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
    private float FindMaxValueIn2DList<T, R>(List<T> val, ExtractDataListFromList<T, R> extractDataDelegate, ExtractIntegerFromData<R> extractIntDelegate)
    {
        int max = 0;

        for (int i = 0; i < val.Count; i++)
        {
            for (int j = 0; j < extractDataDelegate(val[i]).Count; j++)
            {
                int value = extractIntDelegate((extractDataDelegate(val[i]))[j]);
                max = max > value ? max : value;
            }
        }

        return max;
    }
    private float FindMaxValueIn2DList<T, R>(List<T> val, ExtractDataListFromList<T, R> extractDataDelegate, ExtractFloatFromData<R> extractIntDelegate)
    {
        float max = 0;

        for (int i = 0; i < val.Count; i++)
        {
            for (int j = 0; j < extractDataDelegate(val[i]).Count; j++)
            {
                float value = extractIntDelegate((extractDataDelegate(val[i]))[j]);
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
