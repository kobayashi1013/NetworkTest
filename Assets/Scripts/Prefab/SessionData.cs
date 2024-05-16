using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Scenes.Lobby.Manager;

namespace Prefab
{
    public class SessionData : MonoBehaviour
    {
        [SerializeField] private TMP_Text _sessionNameTMP;

        private string _sessionName = "";

        //������
        public void Init(string name)
        {
            _sessionName = name;
            _sessionNameTMP.text = name;
        }

        //�Z�b�V�����ɎQ��
        public async void PushJoinButton()
        {
            //�Z�b�V�������݊m�F
            if (Network.NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            if (Network.NetworkManager.Instance.updatedSessionList.Exists(x => x.Name == _sessionName)) //���݂���
            {
                //�{�^�����b�N
                var buttonList = FindObjectsOfType<Button>();
                foreach (var button in buttonList)
                {
                    button.interactable = false;
                }

                //�Z�b�V�����ɎQ��
                var result = await Network.NetworkManager.Runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Client,
                    Scene = SceneRef.FromIndex((int)Constant.SceneName.InGameScene),
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
                    foreach (var button in buttonList)
                    {
                        button.interactable = true;
                    }
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
