using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurvesWay.Core
{
    public class GameStateController : MonoBehaviour
    {
        public static GameStateController instance = null;
        public delegate void GameStateHandler();
        public event GameStateHandler winGameNotify;
        public event GameStateHandler loseGameNotify;

        public bool HasActivePanel = false;

        public int Money { get; private set; }
        public int Level { get; private set; }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Init();
            }
            else if (instance == this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            
        }

        private void Init()
        {
            Money = PlayerPrefs.GetInt("Money");
            Level = PlayerPrefs.GetInt("Level");
            if(Level == 0)
            {
                Level = 1;
                PlayerPrefs.SetInt("Level", Level);
            }
            FindObjectOfType<StarterScene>().Init();
        }
        
        public void AddMoney(int value)
        {
            Money += value;
            PlayerPrefs.SetInt("Money", Money);
        }

        public void WinGame()
        {
            Level++;
            PlayerPrefs.SetInt("Level", Level);
            winGameNotify?.Invoke();
        }

        public void LoseGame()
        {
            loseGameNotify?.Invoke();
        }

        
    }
}


