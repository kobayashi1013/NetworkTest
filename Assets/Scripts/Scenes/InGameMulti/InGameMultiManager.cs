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

            //�}�C�O���[�V������O
            if (!Object.IsResume)
            {
                //�z�X�g����
                if (NetworkManager.Runner.IsServer)
                {
                    var playerKeys = new List<PlayerRef>(NetworkManager.Instance.playerList.Keys);
                    foreach (var player in playerKeys)
                    {
                        //�v���C���[�Ƃ̕R�Â��X�V
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
            //�z�X�g����
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