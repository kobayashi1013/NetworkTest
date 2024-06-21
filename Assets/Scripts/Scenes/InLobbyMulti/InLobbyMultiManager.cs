using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Network;
using Constant;

namespace Scenes.InLobbyMulti.Manager
{
    public class InLobbyMultiManager : NetworkBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;

        public override void Spawned()
        {
            //Debug.Log("InLobbyMultiScene");

            //�z�X�g����
            if (NetworkManager.Runner.IsServer)
            {
                var playerKeys = new List<PlayerRef>(NetworkManager.Instance.playerList.Keys);
                foreach (var player in playerKeys)
                {
                    //�v���C���[�Ƃ̕R�Â��X�V
                    NetworkObject networkObj = NetworkManager.Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
                    NetworkManager.Instance.playerList[player] = networkObj;
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
                    Runner.LoadScene(SceneRef.FromIndex((int)SceneName.InGameMultiScene));
                }
            }
        }
    }
}
