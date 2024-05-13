using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;

namespace Scenes.LobbyCreate.Manager
{
    public class SessionCreateManager : MonoBehaviour
    {
        private const int _maxPlayer = 2; //��l�Q�[��

        [SerializeField] private GameObject _Canvas;
        [SerializeField] private TMP_InputField _inputField0;
        [SerializeField] private Prefab.Dialog _dialogPrefab;

        //�Z�b�V�����쐬
        public async void OnButton0()
        {
            //�Z�b�V�������d������
            if (Network.NetworkManager.Instance.updatedSessionList.Exists(x => x.Name == _inputField0.text)) //���s
            {
                Debug.Log("exist name");

                //�_�C�A���O�\��
                var obj = Instantiate(_dialogPrefab, _Canvas.transform);
                obj.Init("existed name");
            }
            else //����
            {
                //�{�^�����b�N
                var buttonList = FindObjectsOfType<Button>();
                foreach (var button in buttonList)
                {
                    button.interactable = false;
                }

                //�Z�b�V�����쐬
                var result = await Network.NetworkManager.Runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Host, //�Q�[���ł̌���
                    Scene = SceneRef.FromIndex((int)Constant.SceneName.InGameScene), //���̃Q�[���V�[���̑I��
                    SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(), //Fusion�p��SceneManager�̎w��
                    SessionName = _inputField0.text, //�Z�b�V�������̌���
                    PlayerCount = _maxPlayer //�ő�l���̌���
                });

                if (result.Ok)
                {
                    Debug.Log("Host");
                }
                else
                {
                    Debug.LogError($"error : {result.ShutdownReason}");

                    //���b�N����
                    foreach (var button in buttonList)
                    {
                        button.interactable = true;
                    }
                }
            }
        }

        //Back
        public void OnButton1()
        {
            SceneManager.LoadScene((int)Constant.SceneName.LobbyScene);
        }
    }
}
