using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using Network;
using Constant;
using Prefabs;
using Utils;

namespace Scenes.PrivateConnection.Manager
{
    public class PrivateConnectionManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameObject _canvas;
        [SerializeField] private TMP_InputField _sessionNameInputField;
        [SerializeField] private Button _joinButton;
        [SerializeField] private Button _backButton;
        [Header("Prefabs")]
        [SerializeField] private Dialog _dialogPrefab;

        //�Z�b�V�����Q��
        public async void OnJoinButton()
        {
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");

            //�Z�b�V�����ŐV���擾
            var sessionInfo = NetworkManager.Instance.updatedSessionList.FirstOrDefault(x => x.Name == _sessionNameInputField.text);

            //�Z�b�V�������݊m�F
            if (sessionInfo == null)
            {
                Debug.Log("error : SessionNotExisted");

                //���͍��ڏ�����
                _sessionNameInputField.text = "";

                //�_�C�A���O�\��
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("not found session");

                return;
            }

            //�Z�b�V����������
            if (sessionInfo.PlayerCount == sessionInfo.MaxPlayers)
            {
                Debug.Log("error : SessionPlayer is Max");

                //���͍��ڏ�����
                _sessionNameInputField.text = "";

                //�_�C�A���O�\��
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("session is full");

                return;
            }

            //�{�^�����b�N
            AllButtonLock();

            //�Z�b�V�����ɎQ��
            var args = new StartGameArgs()
            {
                GameMode = GameMode.Client,
                Scene = SceneRef.FromIndex((int)SceneName.InGameScene),
                SceneManager = NetworkManager.Runner.GetComponent<NetworkSceneManagerDefault>(),
                SessionName = _sessionNameInputField.text,
                ConnectionToken = Guid.NewGuid().ToByteArray(),
            };

            var result = await NetworkManager.Instance.JoinSession(NetworkManager.Runner, args);

            //���b�N����
            if (!result) AllButtonRelease();
        }

        //Back
        public void OnBackButton()
        {
            SceneManager.LoadScene((int)SceneName.LobbyScene);
        }

        //�S�Ẵ{�^�������b�N
        private void AllButtonLock()
        {
            _joinButton.interactable = false;
            _backButton.interactable = false;
        }

        //�S�Ẵ{�^�������b�N����
        private void AllButtonRelease()
        {
            _joinButton.interactable = true;
            _backButton.interactable = true;
        }
    }
}
