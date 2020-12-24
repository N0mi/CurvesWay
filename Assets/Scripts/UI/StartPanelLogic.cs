using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using CurvesWay.Core;

public class StartPanelLogic : MonoBehaviour, IPointerDownHandler
{
    private Image background;
    private RectTransform startTxt;
    private RectTransform MoneyRoot;

    private void Awake()
    {        
        background = transform.GetChild(2).GetComponent<Image>();
        startTxt = transform.GetChild(0).GetComponent<RectTransform>();
        MoneyRoot = transform.GetChild(1).GetComponent<RectTransform>();
        MoneyRoot.GetChild(1).GetComponent<Text>().text = GameStateController.instance.Money.ToString();
    }

    private void Start()
    {
        GameStateController.instance.HasActivePanel = true;
        background.DOFade(0, 0.5f);
        startTxt.DOScale(1.1f, 0.6f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DOTween.Kill(startTxt);
        DOTween.Kill(background);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        GameStateController.instance.HasActivePanel = false;
    }
}
