using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Conf;
using System;

namespace CurvesWay.Core
{
    public class StarterScene : MonoBehaviour
    {
        [SerializeField]
        public Color trailClr;

        [SerializeField]
        private Color WayClr;

        [SerializeField]
        private Color WaterClr;
        [SerializeField]
        private Color WaterLineClr;

        [SerializeField]
        private float SpeedUV;


        private Transform canvas;
        private bool isInit = false;
        private GameObject player;

        private void Awake()
        {
            if (GameStateController.instance != null) Init();
        }

        public void Init()
        {          
            canvas = FindObjectOfType<Canvas>().transform;
            Instantiate(Resources.Load<GameObject>(Paths.StartPanel), canvas);
            player = Instantiate(Resources.Load<GameObject>(Paths.Player));
            GetComponent<CameraMover>().ObservableObject = player.transform;
            canvas.GetChild(0).GetComponentInChildren<Text>().text = "LEVEL " + GameStateController.instance.Level;
            GameStateController.instance.winGameNotify += WinAction;
            GameStateController.instance.loseGameNotify += LoseAction;
            SetColor();

        }

        private void SetColor()
        {
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            colorKey = new GradientColorKey[1];
            colorKey[0].color = trailClr;
            colorKey[0].time = 1.0f;

            alphaKey = new GradientAlphaKey[1];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            player.GetComponentInChildren<TrailRenderer>().colorGradient = gradient;

            Resources.Load<Material>(Paths.wayMat).color = WayClr;

            Material ground = Resources.Load<Material>(Paths.groundMat);

            ground.SetColor("_Color", WaterClr);
            ground.SetColor("_ColorLine", WaterLineClr);
            ground.SetFloat("_SpeedUV", SpeedUV);

        }

        private void WinAction()
        {
            GameStateController.instance.winGameNotify -= WinAction;
            Instantiate(Resources.Load<GameObject>(Paths.FinishPanel), canvas);
        }

        private void LoseAction()
        {
            GameStateController.instance.loseGameNotify -= LoseAction;
            Instantiate(Resources.Load<GameObject>(Paths.RestartPanel), canvas);
        }
    }

}
