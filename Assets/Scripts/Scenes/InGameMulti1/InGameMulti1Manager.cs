using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Network;
using Constant;

namespace Scenes.InGameMulti1.Manager
{
    public class InGameMulti1Manager : NetworkBehaviour
    {
        public override void Spawned()
        {
            Debug.Log("Start Multi1");
        }

        public override void Render()
        {
            //ÉzÉXÉgå†å¿
            if (NetworkManager.Runner.IsServer)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Runner.LoadScene(SceneRef.FromIndex((int)SceneName.InGameMulti2));
                }
            }
        }
    }
}