using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Network;

namespace Scenes.InGameMulti2.Manager
{
    public class InGameMulti2Manager : NetworkBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;

        public override void Spawned()
        {
            Debug.Log("Start Multi2");

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
    }
}
