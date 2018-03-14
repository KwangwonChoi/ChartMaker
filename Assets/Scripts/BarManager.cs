using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager{

    public int index
    {
        get;set;
    }

    [HideInInspector]
    public List<Bar> bars = new List<Bar>();



}
