using UnityEngine;
using UnityEngine.UI;

public class ChangeChartScale : MonoBehaviour {

    private const int ChangeSize = 200;

    [SerializeField]
    private ChartMaker _chartMaker;

    private Vector2 originSize;

    private RectTransform rt;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        originSize = rt.sizeDelta;
    }

    //if you use 5 or 6 make type, you should change T,R.
    public void ExtendChartScale()
    {
        rt.sizeDelta += new Vector2(ChangeSize, ChangeSize);

        _chartMaker.Refresh();
    }

    public void ContractChartScale()
    {
        rt.sizeDelta += new Vector2(-ChangeSize, -ChangeSize);

        _chartMaker.Refresh();
    }

    public void OriginalChartScale()
    {
       rt.sizeDelta = originSize;

        _chartMaker.Refresh();
    }
}
