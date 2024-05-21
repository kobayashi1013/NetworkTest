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
        [Header("Prefabs")]
        [SerializeField] private SessionData _sessionDataPrefab;
        [SerializeField] private Dialog _dialogPrefab;

        public static LobbyManager Instance;

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

        //�Z�b�V�����r���[�̍X�V����
        private void SessionViewUpdate()
        {
            //�Z�b�V�����폜
            foreach (Transform child in _sessionListContent.transform)
            {
                Destroy(child.gameObject);
            }

            //�Z�b�V�����`��
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            for (int i = 0; i < NetworkManager.Instance.updatedSessionList.Count; i++)
            {
                var obj = Instantiate(_sessionDataPrefab, _sessionListContent.transform); //�e�I�u�W�F�N�g��ݒ�
                obj.Init(NetworkManager.Instance.updatedSessionList[i].Name); //���̎󂯓n��
            }
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
