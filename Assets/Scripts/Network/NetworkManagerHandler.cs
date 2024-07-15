using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Prefabs;
using Utils;

namespace Network
{
    public class NetworkManagerHandler : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _networkRunnerPrefab;

        public async void ResetNetworkRunner(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            await runner.Shutdown(true, ShutdownReason.HostMigration);
            NetworkManager.Runner = null;
            NetworkManager.Instance = null;

            var newRunner = Instantiate(_networkRunnerPrefab);
            newRunner.ProvideInput = true;
            DontDestroyOnLoad(newRunner);

            await newRunner.StartGame(new StartGameArgs
            {
                SceneManager = newRunner.GetComponent<NetworkSceneManagerDefault>(),
                HostMigrationToken = hostMigrationToken,
                HostMigrationResume = HostMigrationResume,
                ConnectionToken = UserInfo.MyInfo.connectionToken
            });

            Destroy(this.gameObject);
        }

        /// <summary>
        /// 旧セッションのオブジェクトの復元
        /// </summary>
        /// <param name="runner"></param>
        private void HostMigrationResume(NetworkRunner runner)
        {
            foreach (var resumeObj in runner.GetResumeSnapshotNetworkObjects())
            {
                if (resumeObj.TryGetBehaviour<PlayerInfo>(out _)) //プレイヤーオブジェクト
                {
                    if (resumeObj.InputAuthority.PlayerId == resumeObj.GetComponent<PlayerInfo>().hostId)
                    {
                        Debug.Log(resumeObj.InputAuthority.PlayerId);
                        continue;
                    }

                    runner.Spawn(resumeObj, onBeforeSpawned: (_, newObj) =>
                    {
                        newObj.CopyStateFrom(resumeObj);
                        newObj.GetComponent<PlayerInfo>().hostId = runner.LocalPlayer.PlayerId;
                    });
                }
                else //シーンオブジェクト
                {
                    runner.Spawn(resumeObj, onBeforeSpawned: (_, newObj) =>
                    {
                        newObj.CopyStateFrom(resumeObj);
                    });
                }
            }
        }
    }
}
