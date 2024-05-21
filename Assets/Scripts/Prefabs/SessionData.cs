using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Scenes.Lobby.Manager;
using Network;
using Constant;

namespace Prefabs
{
    public class SessionData : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private TMP_Text _sessionNameTMP;

        private string _sessionName = "";

        //������
        public void Init(string name)
        {
            _sessionName = name;
            _sessionNameTMP.text = name;
        }

        //�Z�b�V�����ɎQ��
        public async void OnJoinButton()
        {
            //�Z�b�V�������݊m�F
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            if (NetworkManager.Instance.updatedSessionList.Exists(x => x.Name == _sessionName)) //���݂���
            {
                //�{�^�����b�N
                LobbyManager.Instance.AllButtonLock();

                //�Z�b�V�����ɎQ��
                var result = await NetworkManager.Runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Client,
                    Scene = SceneRef.FromIndex((int)SceneName.InGameScene),
                    SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(),
                    SessionName = _sessionName
                });

                if (result.Ok)
                {
                    Debug.Log("Client");
                }
                else
                {
                    Debug.LogError($"error : {result.ShutdownReason}");

                    //���b�N����
                    LobbyManager.Instance.AllButtonRelease();
                }
            }
            else //�Z�b�V�����͑��݂��Ȃ�
            {
                Debug.Log("error : SessionNotExisted");

                //�\���폜
                if (LobbyManager.Instance == null) Debug.LogError("error : Not Found LobbyManager");
                LobbyManager.Instance.NotExistedSession();
            }
        }
    }
}
