using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Scenes;

namespace Utils
{
    public class SessionInfoCache
    {
        public static SessionInfoCache Instance;

        public ISceneManager sceneManager { get; private set; } //���݂̃V�[���}�l�[�W���[
        public Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>(); //�v���C���[�ƃl�b�g���[�N�I�u�W�F�N�g�̕R�Â�

        public void SetSceneManager(ISceneManager manager)
        {
            //Debug.Log("SceneManager(" + manager ")");
            sceneManager = manager;
        }
    }
}
