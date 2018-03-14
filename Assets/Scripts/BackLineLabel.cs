using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackLineLabel : MonoBehaviour {

    [HideInInspector]
    public Image LineImage;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(LineImage != null)
        rectTransform.position = new Vector3(rectTransform.position.x,LineImage.transform.position.y,0);
    }
}
