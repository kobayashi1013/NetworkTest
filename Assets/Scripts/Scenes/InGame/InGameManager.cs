using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes.InGame.Ball;
using Scenes.InGame.Stick;
using TMPro;
using UniRx;
using Network;
using System;
using Fusion;
using Prefabs;
using Utils;

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

        [SerializeField]
        GameObject _playerPrefab;

        private Subject<Unit> Spawn = new Subject<Unit>();
        public IObservable<Unit> OnSpawn => Spawn;

        public override void Spawned()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (Runner.IsServer && !Runner.IsResume)
            {
                var playerList = new List<PlayerRef>(SessionInfoCache.Instance.playerList.Keys);
                foreach (var player in playerList)
                {
                    var playerObj = SpawnPlayer(Runner, player);
                    SessionInfoCache.Instance.playerList[player] = playerObj;
                }

            }
        }

        public NetworkObject SpawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            var position = new Vector3(UnityEngine.Random.Range(0, 100), 0, 0);

            var playerObj = runner.Spawn(
                _playerPrefab,
                position,
                Quaternion.identity,
                player,
                (_, obj) =>
                {
                    var playerInfo = obj.GetComponent<PlayerInfo>();
                    var objectTokenCs = obj.GetComponent<NetworkObjectTrackingData>();
                    var objectToken = "token";
                    if (runner.LocalPlayer == player) objectToken = "HOST";
                    else objectToken = Guid.NewGuid().ToString();

                    playerInfo.connectionToken = new Guid(runner.GetPlayerConnectionToken(player)).GetHashCode();
                    objectTokenCs.token = objectToken;
                });

            return playerObj;
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