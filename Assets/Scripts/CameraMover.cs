using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CurvesWay.Core;

public class CameraMover : MonoBehaviour
{
    public Transform ObservableObject;
    [SerializeField] private float _damper = 10f;
    [SerializeField] private Vector3 _offsetCam = new Vector3(0, 7, -5);

    private bool isStop = false;

    private void Start()
    {
        transform.position = ObservableObject.position + _offsetCam;
        GameStateController.instance.winGameNotify += WinGameAction;
    }

    private void FixedUpdate()
    {
        if(!isStop)
            transform.position = Vector3.Lerp(transform.position, ObservableObject.position + _offsetCam, _damper * Time.deltaTime);
    }

    public void setDamper(float value)
    {
        _damper = value;
    }

    public void setObservableTarget(Transform obj)
    {
        ObservableObject = obj;
    }

    public void WinGameAction()
    {
        
        isStop = true;
        Vector3 rotation = transform.eulerAngles;
        transform.DORotate(new Vector3(rotation.x, rotation.y, rotation.z + 360), 40, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Flash);
    }

    private void OnDestroy()
    {
        GameStateController.instance.winGameNotify -= WinGameAction;
    }
}
