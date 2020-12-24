using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CurvesWay.Tools.Way;

namespace CurvesWay.Core
{
    public class CubeMover : MonoBehaviour
    {
        public bool lookForward;

        [SerializeField]
        private float stepsPerSpline = 2f;
        [SerializeField]
        private float MaxSpeed = 20f;
        [SerializeField]
        private float Acceleration = 1f;

        private float EndProgress = 0.9f;


        private float Speed = 0f;
        private WaySplineCreator way;
        public float progress = 0f;
        private Vector3 targetPos;
        private bool isGo = false;
        private bool lowSpeed = false;
        private Rigidbody rb;

        private void Awake()
        {
            way = FindObjectOfType<WaySplineCreator>();
            EndProgress = way.EndProgress;
            GetComponentInChildren<TrailRenderer>().time = Mathf.Infinity;
            rb = GetComponent<Rigidbody>();
            progress = way.StartProgress;
            transform.localPosition = way.GetPoint(progress) + Vector3.up * 0.5f + Quaternion.LookRotation(way.GetDirection(progress)) * Vector3.forward * -1f;
        }

        // Update is called once per frame
        void Update()
        {
            if(!GameStateController.instance.HasActivePanel)
            {
                if (Input.GetMouseButton(0) || Input.touchCount > 0)
                {
                    isGo = true;
                }
                else
                {
                    isGo = false;
                }
            }



            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if ((targetPos == Vector3.zero || Vector3.Magnitude(targetPos - transform.position) < 1f))
            {
                targetPos = NextPoint();
            }

        }

        private void FixedUpdate()
        {

            if ((!isGo && progress < EndProgress) || progress == 1)
            {
                Speed = Mathf.Lerp(Speed, 0, Acceleration / 5);             
            }
            else if(lowSpeed)
            {
                Speed = MaxSpeed / 2;
            }
            else
            {
                Speed = Mathf.Lerp(Speed, MaxSpeed, Acceleration / 10);
            }

            rb.velocity = (targetPos - transform.position).normalized * Speed;

            if (lookForward)
            {
                transform.LookAt(targetPos);
            }
        }

        private Vector3 NextPoint()
        {
            progress += 1 / stepsPerSpline;
            if (progress > EndProgress)
            {
                lowSpeed = true;
            }

            if(progress > 1)
            {
                progress = 1;
                GameStateController.instance.WinGame();
                rb.velocity = Vector3.zero;
                Destroy(this);
            }

            return way.GetPoint(progress) + Vector3.up * 0.5f;
        }
    }
}

