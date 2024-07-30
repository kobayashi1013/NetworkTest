using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Scenes;
using Utils;

namespace Network
{
    public class HostMigrationHandler : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _runnerPrefab;
        private List<string> _resumeTokens = new List<string>();

        /// <summary>
        /// Runnerを新しいものに変更
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="hostMigrationToken"></param>
        public async void RebootRunner(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            //自分のプレイヤートークンを取得
            var connectionTokenBytes = runner.GetPlayerConnectionToken(runner.LocalPlayer);

            //オブジェクトトークンを取得
            var tokens = FindObjectsOfType<NetworkObjectTrackingData>().Select(x => x.token);
            _resumeTokens = new List<string>(tokens);

            //Runnerの再起動
            await runner.Shutdown(true, ShutdownReason.HostMigration);
            NetworkManager.Instance = null;
            NetworkManager.Runner = null;

            runner = Instantiate(_runnerPrefab);
            runner.ProvideInput = true;

            var args = new StartGameArgs
            {
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = runner.GetComponent<NetworkSceneManagerDefault>(),
                HostMigrationToken = hostMigrationToken,
                HostMigrationResume = HostMigrationResume,
                ConnectionToken = connectionTokenBytes,
            };
            
            await NetworkManager.Instance.JoinSession(runner, args);

            Destroy(this.gameObject);
        }

        /// <summary>
        /// ホスト遷移後に実行される
        /// </summary>
        /// <param name="runner"></param>
        private void HostMigrationResume(NetworkRunner runner)
        {
            foreach (var resumeObj in runner.GetResumeSnapshotNetworkObjects())
            {
                //ネットワークオブジェクトの情報を取得
                var networkObjectTrackingData = resumeObj.GetComponent<NetworkObjectTrackingData>();
                var objectToken = networkObjectTrackingData.token;
                var position = networkObjectTrackingData.position;
                var rotation = networkObjectTrackingData.rotation;

                //旧ホストオブジェクトはスポーンしない
                if (objectToken == "HOST") continue;

                if (_resumeTokens.Exists(x => x == objectToken))
                {
                    //シーンマネージャーオブジェクト
                    if (resumeObj.TryGetComponent<ISceneManager>(out _))
                    {
                        runner.Spawn(resumeObj, position, rotation, null, (_, newObj) =>
                        {
                            newObj.CopyStateFrom(resumeObj);
                            SessionInfoCache.Instance.SetSceneManager(newObj.GetComponent<ISceneManager>()); //次のシーンオブジェクトを登録
                        });
                    }
                    else
                    {
                        runner.Spawn(resumeObj, position, rotation, null, (_, newObj) =>
                        {
                            newObj.CopyStateFrom(resumeObj);
                        });
                    }
                }
            }
        }
    }
}