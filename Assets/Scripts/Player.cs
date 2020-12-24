using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurvesWay.Core
{
    public class Player : MonoBehaviour
    {

        private void Awake()
        {
            Init();
        }

        private void Init()
        {

        }

        public void Death()
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("Lose");
        }

        private void SendAction()
        {
            GameStateController.instance.LoseGame();
        }

        private void OnCollisionEnter(Collision collision)
        {            
            Death();
            Invoke("SendAction", 1f);
        }
    }
}


