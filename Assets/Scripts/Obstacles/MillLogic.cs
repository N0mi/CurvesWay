using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MillLogic : MonoBehaviour
{
    [SerializeField]
    private float Speed = 1;
    [SerializeField]
    private bool Clockwise = true;
    private Transform blades;

    private void Start()
    {        
        blades = transform.GetChild(1);

        float rotChanger = -1;
        if (Clockwise) rotChanger = 1;
        if(Speed > 0)
            blades.DORotate(new Vector3(0, blades.eulerAngles.y + 360 * rotChanger, 0), 10f / Speed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Flash);
    }
}
