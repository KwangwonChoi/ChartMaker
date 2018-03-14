using UnityEngine;
using UnityEngine.UI;

public class DataPanel : MonoBehaviour {

    [SerializeField]
    private Text _dataText;
    
	// Update is called once per frame
	private void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(this.gameObject);
        }	
	}

    public void SetText(string str)
    {
        _dataText.text = str;
    }
}
