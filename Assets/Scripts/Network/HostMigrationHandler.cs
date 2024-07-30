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
        /// Runner��V�������̂ɕύX
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="hostMigrationToken"></param>
        public async void RebootRunner(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            //�����̃v���C���[�g�[�N�����擾
            var connectionTokenBytes = runner.GetPlayerConnectionToken(runner.LocalPlayer);

            //�I�u�W�F�N�g�g�[�N�����擾
            var tokens = FindObjectsOfType<NetworkObjectTrackingData>().Select(x => x.token);
            _resumeTokens = new List<string>(tokens);

            //Runner�̍ċN��
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
        /// �z�X�g�J�ڌ�Ɏ��s�����
        /// </summary>
        /// <param name="runner"></param>
        private void HostMigrationResume(NetworkRunner runner)
        {
            foreach (var resumeObj in runner.GetResumeSnapshotNetworkObjects())
            {
                //�l�b�g���[�N�I�u�W�F�N�g�̏����擾
                var networkObjectTrackingData = resumeObj.GetComponent<NetworkObjectTrackingData>();
                var objectToken = networkObjectTrackingData.token;
                var position = networkObjectTrackingData.position;
                var rotation = networkObjectTrackingData.rotation;

                //���z�X�g�I�u�W�F�N�g�̓X�|�[�����Ȃ�
                if (objectToken == "HOST") continue;

                if (_resumeTokens.Exists(x => x == objectToken))
                {
                    //�V�[���}�l�[�W���[�I�u�W�F�N�g
                    if (resumeObj.TryGetComponent<ISceneManager>(out _))
                    {
                        runner.Spawn(resumeObj, position, rotation, null, (_, newObj) =>
                        {
                            newObj.CopyStateFrom(resumeObj);
                            SessionInfoCache.Instance.SetSceneManager(newObj.GetComponent<ISceneManager>()); //���̃V�[���I�u�W�F�N�g��o�^
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