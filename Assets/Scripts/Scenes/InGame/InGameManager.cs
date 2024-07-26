using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes.InGame.Ball;
using Scenes.InGame.Stick;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes.InGame.Ball;
using Scenes.InGame.Stick;
using TMPro;
using UniRx;
using System;
using System.Diagnostics;
using Fusion;

namespace Scenes.InGame.Manager
{
    public class InGameManager : NetworkBehaviour
    {
        BallSpawner _ballSpawner;
        BallStatus _ballStatus;
        StickStatus _stickStatus;
        public static InGameManager Instance;

        [Networked, OnChangedRender(nameof(OnScoreChanged))]
        private int _score { get; set; } = 0;//スコア
        private int _blockSize = 0;//blockの数
        [SerializeField, Tooltip("スコアを表示するUI")]
        TextMeshProUGUI _socreText;

        private Subject<Unit> Spawn = new Subject<Unit>();
        public IObservable<Unit> OnSpawn => Spawn;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void OnScoreChanged()
        {
            UnityEngine.Debug.Log(_score);
            _blockSize--;
            _socreText.text = $"SCORE:{_score}";
            if (_blockSize <= 0)
            {
                GameOver();
            }
        }

        /*public override void Spawned()
        {
            UnityEngine.Debug.Log("a");
            stickSpawner.SpawnPlayers(Runner);
        }*/

        void Start()
        {
            _ballSpawner = GetComponent<BallSpawner>();
            StartCoroutine(BallSpawn());
        }


        IEnumerator BallSpawn()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            Spawn.OnNext(default);
            
        }

        /*public override void FixedUpdateNetwork()
        {
            UnityEngine.Debug.Log("Spawn3");
            if (GetInput(out NetworkInputData data))
            {
                UnityEngine.Debug.Log("Spawn2");
                if (data.Buttons.IsSet(NetworkInputButtons.Space))
                {
                    UnityEngine.Debug.Log("Spawn");
                    Spawn.OnNext(default);    
                    Spawn.Dispose();
                }

            }
        }*/

        public void GameOver()
        {
            _ballStatus = FindObjectOfType<BallStatus>();
            _stickStatus = FindObjectOfType<StickStatus>();
            _ballStatus.isDestroy();
            _stickStatus.StopMove();
        }
        public void BlockSize(int i)
        {
            _blockSize = i;
        }
        public void BlockDestroy()
        {
            _score += 100;

        }

        public void ChangeScore(int score)
        {
            _score += score;
            _socreText.text = $"SCORE:{_score}";
        }
    }
}