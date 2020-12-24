using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Conf;
using CurvesWay.Core;
using UnityEngine.EventSystems;

public class FinishPanelLogic : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private float durationJumpRootMoney = 0.1f;

    [SerializeField]
    private float durationMoneyMin = 0.1f;

    [SerializeField]
    private float durationMoneyMax = 1f;

    private GameObject MoneyPref;
    private Image background;
    private RectTransform startTxt;
    private RectTransform MoneyRoot;
    private RectTransform MoneyRootImg;
    private Vector3 startpos;
    private Queue<GameObject> moneyQueue = new Queue<GameObject>();
    private int money = 0;
    private bool isPress = false;

    void Start()
    {
        GameStateController.instance.HasActivePanel = true;
        background = transform.GetChild(3).GetComponent<Image>();
        startTxt = transform.GetChild(0).GetComponent<RectTransform>();
        MoneyPref = Resources.Load<GameObject>(Paths.MoneyOne);
        MoneyRoot = transform.GetChild(1).GetComponent<RectTransform>();
        money = GameStateController.instance.Money;
        MoneyRoot.GetChild(1).GetComponent<Text>().text = money.ToString();
        MoneyRootImg = MoneyRoot.GetChild(0).GetComponent<RectTransform>();
        startpos = MoneyRoot.localPosition;
        startTxt.DOScale(1.1f, 0.6f).SetLoops(-1, LoopType.Yoyo);    
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddMoney(25);
        }
    }

    private void GenerateMoney(int value)
    {
        for (int i = 0; i < value; i++)
        {
            GameObject go = Instantiate(MoneyPref, transform);
            RectTransform trans = go.GetComponent<RectTransform>();

            float offsetX = Mathf.Tan((float)Random.Range(-100, 100) / 100);
            float offsetY = Mathf.Tan((float)Random.Range(-100, 100) / 100);
            Vector3 offset = new Vector3(offsetX, offsetY, 0) * 200;

            trans.DOMove(trans.position + offset, 0.5f).
                OnComplete(() => trans.DOMove(MoneyRootImg.position, Random.Range(durationMoneyMin, durationMoneyMax)).
                OnComplete(() =>
                {
                    Destroy(go);
                    AddViewMoney(1);
                }));
        }
    }

    private void AddMoney(int value)
    {
        GameStateController.instance.AddMoney(value);
        GenerateMoney(value);
    }

    public void AddViewMoney(int value)
    {
        money+=value;
        MoneyRoot.GetChild(1).GetComponent<Text>().text = money.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isPress)
        {
            isPress = true;
            AddMoney(50);
            Invoke("NextLevel", durationMoneyMax + 0.5f);
        }        
    }


    private void NextLevel()
    {
        background.DOFade(1, 0.5f).OnComplete(() => SceneLoader.instance.NextScene());
    }

    private void OnDestroy()
    {
        GameStateController.instance.HasActivePanel = false;
    }
}
