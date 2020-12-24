using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CurvesWay.Tools.Way;
using CurvesWay.Core;


public class ProgressController : MonoBehaviour
{
    
    private Slider slider;

    private CubeMover player;

    private float minOffset;
    private float maxOffset;

    private void Start()
    {
        WaySplineCreator way = FindObjectOfType<WaySplineCreator>();
        slider = GetComponent<Slider>();
        SetColor(FindObjectOfType<StarterScene>().trailClr);
        maxOffset = way.EndProgress;
        minOffset = way.StartProgress;
        player = FindObjectOfType<CubeMover>();
    }

    private void SetColor(Color clr)
    {
        transform.GetChild(1).GetChild(0).GetComponent<Image>().color = clr;
    }

    private void Update()
    {
        slider.value = player.progress;
    }
}
