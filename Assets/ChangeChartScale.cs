using UnityEngine;
using UnityEngine.UI;

public class ChangeChartScale : MonoBehaviour {

    private const int ChangeSize = 50;

    [SerializeField]
    private BarChart barChart;

    RectTransform rt;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    public void ExtendChartScale()
    {
        rt.sizeDelta += new Vector2(ChangeSize, ChangeSize);
        barChart.UpdateLines(barChart.tmpList, barChart.nameTmpList);
    }

    public void ContractChartScale()
    {
        rt.sizeDelta += new Vector2(-ChangeSize, -ChangeSize);
        barChart.UpdateLines(barChart.tmpList, barChart.nameTmpList);
    }
}
