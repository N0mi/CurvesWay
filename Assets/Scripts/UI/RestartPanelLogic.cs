using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CurvesWay.Core;
using DG.Tweening;

public class RestartPanelLogic : MonoBehaviour, IPointerDownHandler
{
    private Image background;
    private RectTransform startTxt;
    private RectTransform MoneyRoot;

    private void Awake()
    {
        GameStateController.instance.HasActivePanel = true;
        background = transform.GetChild(2).GetComponent<Image>();
        startTxt = transform.GetChild(0).GetComponent<RectTransform>();
        MoneyRoot = transform.GetChild(1).GetComponent<RectTransform>();
        MoneyRoot.GetChild(1).GetComponent<Text>().text = GameStateController.instance.Money.ToString();
        startTxt.DOScale(1.1f, 0.6f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.DOFade(1, 0.5f).OnComplete(() => SceneLoader.instance.RestartGame());
    }

    private void OnDestroy()
    {
        GameStateController.instance.HasActivePanel = false;
    }
}
