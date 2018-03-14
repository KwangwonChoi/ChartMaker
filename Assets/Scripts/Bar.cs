using System;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

    public static int i = 0;

    public Image bar;
    public Image circle;
    public Image line;
    public Text barValue;

    [HideInInspector]
    public object data;//This should be data.

    [SerializeField]
    private DataPanel _dataPanelPrefab;

    public void ShowButtonData() {
        DataPanel dataPanel = Instantiate(_dataPanelPrefab) as DataPanel;
        dataPanel.gameObject.SetActive(true);
        RectTransform dataRect = dataPanel.GetComponent<RectTransform>();
        dataPanel.transform.SetParent(FindObjectOfType<Canvas>().transform);
        dataPanel.transform.position = circle.transform.position + new Vector3(-dataRect.rect.width / 2,-dataRect.rect.height / 2, 0);
        
        dataPanel.SetText(data.ToString());
    }
}
