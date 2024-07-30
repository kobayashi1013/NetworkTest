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

        public ISceneManager sceneManager { get; private set; } //現在のシーンマネージャー
        public Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>(); //プレイヤーとネットワークオブジェクトの紐づけ

        public void SetSceneManager(ISceneManager manager)
        {
            //Debug.Log("SceneManager(" + manager ")");
            sceneManager = manager;
        }
    }
}
