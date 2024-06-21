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

            //ホスト権限
            if (NetworkManager.Runner.IsServer)
            {
                var playerKeys = new List<PlayerRef>(NetworkManager.Instance.playerList.Keys);
                foreach (var player in playerKeys)
                {
                    //プレイヤーとの紐づけ更新
                    NetworkObject networkObj = NetworkManager.Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
                    NetworkManager.Instance.playerList[player] = networkObj;
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
                    Runner.LoadScene(SceneRef.FromIndex((int)SceneName.InGameMultiScene));
                }
            }
        }
    }
}
