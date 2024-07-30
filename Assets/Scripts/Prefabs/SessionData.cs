using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Scenes.Lobby.Manager;
using Network;
using Constant;
using Utils;

namespace Prefabs
{
    public class SessionData : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private TMP_Text _sessionNameTMP;
        [SerializeField] private TMP_Text _sessionPlayerCountTMP;

        private SessionInfo _sessionInfo;

        //������
        public void Init(SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;

            _sessionNameTMP.text = _sessionInfo.Name;
            _sessionPlayerCountTMP.text = _sessionInfo.PlayerCount.ToString() + "/" + _sessionInfo.MaxPlayers.ToString();
        }

        //�Z�b�V�����ɎQ��
        public async void OnJoinButton()
        {
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            if (LobbyManager.Instance == null) Debug.LogError("error : Not Found LobbyManager");

            //�Z�b�V�����ŐV���擾
            var sessionInfo = NetworkManager.Instance.updatedSessionList.FirstOrDefault(x => x.Name == _sessionInfo.Name);

            //�Z�b�V�������݊m�F
            if (sessionInfo == null)
            {
                Debug.Log("error : SessionNotExisted");
                LobbyManager.Instance.NotExistedSession(); //�_�C�A���O

                return;
            }

            //�Z�b�V����������
            if (sessionInfo.PlayerCount == sessionInfo.MaxPlayers)
            {
                Debug.Log("error : SessionPlayer is Max");
                LobbyManager.Instance.SessionPlayerMax();

                return;
            }

            //�{�^�����b�N
            LobbyManager.Instance.AllButtonLock();

            //�Z�b�V�����ɎQ��
            var args = new StartGameArgs()
            {
                GameMode = GameMode.Client,
                Scene = SceneRef.FromIndex((int)SceneName.InLobbyMultiScene),
                SceneManager = NetworkManager.Instance.GetComponent<NetworkSceneManagerDefault>(),
                SessionName = _sessionInfo.Name,
                ConnectionToken = Guid.NewGuid().ToByteArray(),
            };

            var result = await NetworkManager.Instance.JoinSession(NetworkManager.Runner, args);

            //���b�N����
            if (!result) LobbyManager.Instance.AllButtonRelease();
        }
    }
}
