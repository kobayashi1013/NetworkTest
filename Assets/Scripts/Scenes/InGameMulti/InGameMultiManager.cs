using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Network;
using Constant;
using System;
using Prefabs;

namespace Scenes.InGameMulti.Manager
{
    public class InGameMultiManager : NetworkBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;

        public override void Spawned()
        {
            //Debug.Log("InLobbyMultiScene");

            //マイグレーション例外
            if (!Object.IsResume)
            {
                //ホスト権限
                if (NetworkManager.Runner.IsServer)
                {
                    var playerKeys = new List<PlayerRef>(NetworkManager.Instance.playerList.Keys);
                    foreach (var player in playerKeys)
                    {
                        //プレイヤーとの紐づけ更新
                        NetworkObject networkObj = NetworkManager.Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
                        var playerInfo = networkObj.GetComponent<PlayerInfo>();
                        playerInfo.connectionToken = new Guid(NetworkManager.Runner.GetPlayerConnectionToken(player)).GetHashCode();
                        playerInfo.hostId = NetworkManager.Runner.LocalPlayer.PlayerId;
                        NetworkManager.Instance.playerList[player] = networkObj;
                    }
                }
            }
        }

        public override void Render()
        {
            //ホスト権限
            if (NetworkManager.Runner.IsServer)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Runner.LoadScene(SceneRef.FromIndex((int)SceneName.InLobbyMultiScene));
                }
            }
        }
    }
}