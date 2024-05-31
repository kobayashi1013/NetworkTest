using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using Network;
using Prefabs;
using Constant;

namespace Scenes.Lobby.Manager
{
    public class LobbyManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameObject _canvas;
        [SerializeField] private GameObject _sessionListContent;
        [SerializeField] private TMP_Text _lobbyPlayersTMP;
        [Header("Prefabs")]
        [SerializeField] private SessionData _sessionDataPrefab;
        [SerializeField] private Dialog _dialogPrefab;
        [SerializeField] private ToSoloModeDialog _toSoloModeDialogPrefab;

        public static LobbyManager Instance;
        private int _lobbyPlayers = 0; //���r�[�̐l��

        void Awake()
        {
            //�C���X�^���X��
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            //�Z�b�V�����\���X�V
            SessionViewUpdate();
        }

        //Create Session
        public void OnCreateSessionButton()
        {
            SceneManager.LoadScene((int)SceneName.SessionCreateScene);
        }

        //Back
        public void OnBackButton()
        {
            //�S�{�^�����b�N
            AllButtonLock();

            //���r�[���痣�E
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            NetworkManager.Runner.Shutdown();

            //�X�^�[�g���j���[�V�[���֑J��
            SceneManager.LoadScene((int)SceneName.StartMenuScene);
        }

        //�Z�b�V�����r���[�̍X�V
        public void OnUpdateButton()
        {
            SessionViewUpdate();
        }

        public void OnPrivateConnectionButton()
        {
            //�v���C�x�[�g�Q���V�[���֑J��
            SceneManager.LoadScene((int)SceneName.PrivateConnectionScene);
        }

        //�Z�b�V�����r���[�̍X�V����
        private void SessionViewUpdate()
        {
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");

            //���r�[�̐l����������
            _lobbyPlayers = 0;

            //�Z�b�V�����폜
            foreach (Transform child in _sessionListContent.transform)
            {
                Destroy(child.gameObject);
            }

            //�Z�b�V�����`��
            for (int i = 0; i < NetworkManager.Instance.updatedSessionList.Count; i++)
            {
                //�Z�b�V�����ŐV���擾
                var sessionInfo = NetworkManager.Instance.updatedSessionList[i];

                //���r�[�l���J�E���g
                _lobbyPlayers += sessionInfo.PlayerCount;

                //�Z�b�V�����\��
                if (sessionInfo.Properties.TryGetValue("visible", out var property))
                {
                    bool isVisible = (bool)property.PropertyValue;
                    if (isVisible && sessionInfo.PlayerCount != sessionInfo.MaxPlayers) //Public�ł���
                    {
                        var obj = Instantiate(_sessionDataPrefab, _sessionListContent.transform); //�e�I�u�W�F�N�g��ݒ�
                        obj.Init(sessionInfo); //���̎󂯓n��
                    }
                }
            }

            //���r�[�l���\��
            _lobbyPlayersTMP.text = _lobbyPlayers.ToString();
        }

        //�Z�b�V���������݂��Ȃ�
        public void NotExistedSession()
        {
            //�_�C�A���O�\��
            var obj = Instantiate(_dialogPrefab, _canvas.transform);
            obj.Init("not found session");

            //�Z�b�V�����r���[�X�V
            SessionViewUpdate();
        }

        //�Z�b�V�����l�������
        public void SessionPlayerMax()
        {
            var obj = Instantiate(_toSoloModeDialogPrefab, _canvas.transform);
            obj.Init("session is full");

            //�Z�b�V�����r���[�X�V
            SessionViewUpdate();
        }

        //�S�Ẵ{�^�������b�N
        public void AllButtonLock()
        {
            var allButtonList = FindObjectsOfType<Button>();
            foreach (var button in allButtonList)
            {
                button.interactable = false;
            }
        }

        //�S�Ẵ{�^�������b�N����
        public void AllButtonRelease()
        {
            var allButtonList = FindObjectsOfType<Button>();
            foreach (var button in allButtonList)
            {
                button.interactable = true;
            }
        }
    }
}
