using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using Prefabs;
using Network;
using Constant;
using Utils;

namespace Scenes.LobbyCreate
{
    public class SessionCreateManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameObject _canvas;
        [SerializeField] private TMP_InputField _sessionNameInputField;
        [SerializeField] private Toggle _isVisibleToggle;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _backButton;
        [Header("Prefabs")]
        [SerializeField] private Dialog _dialogPrefab;
        [Header("Parameters")]
        [SerializeField] private int _maxPlayer = 2;
        [SerializeField] private int _sessionNameMin = 1;
        [SerializeField] private int _sessionNameMax = 12;

        //�Z�b�V�����쐬
        public async void OnPlayButton()
        {
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");

            //�Z�b�V�������d������
            if (NetworkManager.Instance.updatedSessionList.Exists(x => x.Name == _sessionNameInputField.text)) //���s
            {
                Debug.Log("exist name");

                //���͍��ڏ�����
                _sessionNameInputField.text = "";

                //�_�C�A���O�\��
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("existed name");

                return;
            }

            //����������
            if (_sessionNameInputField.text.Length < _sessionNameMin ||
                _sessionNameInputField.text.Length > _sessionNameMax)
            {
                Debug.Log("word count error");

                //���͍��ڏ�����
                _sessionNameInputField.text = "";

                //�_�C�A���O�\��
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("word count error");

                return;
            }

            //�{�^�����b�N
            AllButtonLock();

            //�J�X�^���v���p�e�B
            var customProps = new Dictionary<string, SessionProperty>();
            customProps["visible"] = !_isVisibleToggle.isOn; //���E�s��

            //�Z�b�V�����쐬
            var args = new StartGameArgs()
            {
                GameMode = GameMode.Host, //�Q�[���ł̌���
                Scene = SceneRef.FromIndex((int)SceneName.InGameScene), //���̃Q�[���V�[���̑I��
                SceneManager = NetworkManager.Runner.GetComponent<NetworkSceneManagerDefault>(), //Fusion�p��SceneManager�̎w��
                SessionName = _sessionNameInputField.text, //�Z�b�V�������̌���
                SessionProperties = customProps,
                PlayerCount = _maxPlayer, //�ő�l���̌���
                ConnectionToken = Guid.NewGuid().ToByteArray(), //�v���C���[�g�[�N���̌���
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
            _playButton.interactable = false;
            _backButton.interactable = false;
        }

        //�S�Ẵ{�^��������
        private void AllButtonRelease()
        {
            _playButton.interactable = true;
            _backButton.interactable = true;
        }
    }
}
